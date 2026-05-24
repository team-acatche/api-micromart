using AuthenticationApi.Core;
using Scalar.AspNetCore;
using AuthenticationApi.Models;
using AuthenticationApi.Models.Errors;
using AuthenticationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IBasicAuthenticator, Sha256BasicAuthenticator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapPost("/login", IResult (IBasicAuthenticator authenticator, LoginRequest loginRequest) =>
{
    var userToken = authenticator.Login(loginRequest.username, loginRequest.password);
    if (userToken == null) return TypedResults.Unauthorized();
    
    var user = authenticator.GetUserFromToken(userToken.TokenString)!;
    return TypedResults.Ok(new ApiResponse<LoginResponse>
    {
        ComponentName = "Authenticator",
        Success = true,
        Result = new LoginResponse(userToken.TokenString, user.UserId, user.Username),
        Error = null,
    });
})
    .Produces<ApiResponse<LoginResponse>>()
    .Produces(StatusCodes.Status401Unauthorized)
    .WithName("Login");

app.MapPost("/register", IResult (IBasicAuthenticator authenticator, RegisterRequest request) =>
    {
        var user = authenticator.Register(request.Username, request.Password);
        if (user == null) return TypedResults.BadRequest(new ApiResponse<RegisterResponse?>
        {
            ComponentName = "Authenticator",
            Success = false,
            Error = new UsernameAlreadyTakenError(),
            Result = null,
        });
        
        return TypedResults.Ok(new ApiResponse<RegisterResponse>
        {
            ComponentName = "Authenticator",
            Success = true,
            Result = new RegisterResponse(user.UserId, user.Username),
            Error = null,
        });
    })
    .Produces<ApiResponse<RegisterResponse>>()
    .Produces<ApiResponse<RegisterResponse?>>(StatusCodes.Status400BadRequest)
    .WithName("Register");

app.Run();
