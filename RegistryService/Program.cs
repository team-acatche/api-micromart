using RegistryService;
using RegistryService.Core;
using RegistryService.Models;
using RegistryService.Models.Errors;
using RegistryService.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSingleton<IRegistryService>(new BaseRegistryService()
{
    Components = BaseRegistryComponents.Components,
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var api = app.MapGroup("api/registry");

api.MapGet("/components", IResult (IRegistryService registryService) => TypedResults.Ok(
    new ApiResponse<List<RegistryComponent>>()
    {
        ComponentName = "Registry",
        Success = true,
        Result = registryService.GetComponents(),
    }))
    .Produces<ApiResponse<List<RegistryComponent>>>()
    .WithName("GetComponents");

api.MapGet("/components/{name}", IResult (IRegistryService registryService, string name) =>
    {
        var matchingComponent = registryService.GetComponentWithName(name);
        if (matchingComponent == null)
        {
            return TypedResults.NotFound(new ApiResponse<RegistryComponent>()
            {
                ComponentName = "Registry",
                Success = false,
                Error = new ComponentNotFoundError(name),
            });
        }
        
        return TypedResults.Ok(new ApiResponse<RegistryComponent>()
        {
            ComponentName = "Registry",
            Success = true,
            Result = matchingComponent
        });
    })
    .Produces<ApiResponse<RegistryComponent>>()
    .Produces<ApiResponse<RegistryComponent?>>(StatusCodes.Status404NotFound)
    .WithName("GetComponent");

app.Run();