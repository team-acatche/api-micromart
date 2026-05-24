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

var api = builder.Build();

// Configure the HTTP request pipeline.
if (api.Environment.IsDevelopment())
{
    api.MapOpenApi();
    api.MapScalarApiReference();
}

api.UseHttpsRedirection();

var app = api.MapGroup("api/authenticate");

app.MapPost("/login", IResult (IBasicAuthenticator authenticator, LoginRequest request) =>
{
    if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
    {
        string[] fieldNames = ["username", "password"];
        bool[] emptyFieldsFlag = [string.IsNullOrEmpty(request.Username), string.IsNullOrEmpty(request.Password)];

        var emptyFields = fieldNames.Where(((_, idx) => emptyFieldsFlag[idx]))
            .ToArray();

        return TypedResults.BadRequest(new ApiResponse<LoginResponse?>()
        {
            ComponentName = "Authenticator",
            Success = false,
            Error = new EmptyFieldError(emptyFields),
        });
    }
    
    var userToken = authenticator.Login(request.Username, request.Password);
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
    .Produces<ApiResponse<LoginResponse?>>(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status401Unauthorized)
    .WithName("Login");

app.MapPost("/register", IResult (IBasicAuthenticator authenticator, RegisterRequest request) =>
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            string[] fieldNames = ["username", "password"];
            bool[] emptyFieldsFlag = [string.IsNullOrEmpty(request.Username), string.IsNullOrEmpty(request.Password)];

            var emptyFields = fieldNames.Where(((_, idx) => emptyFieldsFlag[idx]))
                .ToArray();

            return TypedResults.BadRequest(new ApiResponse<RegisterResponse?>()
            {
                ComponentName = "Authenticator",
                Success = false,
                Error = new EmptyFieldError(emptyFields),
            });
        }
        
        var user = authenticator.Register(request.Username, request.Password);
        if (user == null) return TypedResults.BadRequest(new ApiResponse<RegisterResponse?>
        {
            ComponentName = "Authenticator",
            Success = false,
            Error = new UsernameAlreadyTakenError(),
        });
        
        return TypedResults.Ok(new ApiResponse<RegisterResponse>
        {
            ComponentName = "Authenticator",
            Success = true,
            Result = new RegisterResponse(user.UserId, user.Username),
        });
    })
    .Produces<ApiResponse<RegisterResponse>>()
    .Produces<ApiResponse<RegisterResponse?>>(StatusCodes.Status400BadRequest)
    .WithName("Register");

api.Run();
