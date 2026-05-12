using Scalar.AspNetCore;

using TaxCalculatorApi;
using TaxCalculatorApi.Core;
using TaxCalculatorApi.Models;
using TaxCalculatorApi.Models.Errors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var api = builder.Build();

// Configure the HTTP request pipeline.
if (api.Environment.IsDevelopment())
{
    api.MapOpenApi();
    api.MapScalarApiReference();
}

api.UseHttpsRedirection();

var app = api.MapGroup("api/v1");

app.MapPost("/calculate", IResult (TaxCalculationRequest request) =>
    {
        if (request.Price < 0)
        {
            return TypedResults.BadRequest(new ApiResponse<TaxCalculationResponse>()
            {
                ComponentName = "TaxCalculator",
                Success = false,
                Error = new InitialPriceBelowZeroError(request.Price),
            });
        }
        
        var totalPrice = TaxCalculator.Calculate(request.Price, request.RegionCode);
        return TypedResults.Ok(new ApiResponse<TaxCalculationResponse>()
        {
            ComponentName = "TaxCalculator",
            Success = true,
            Result = new TaxCalculationResponse(request, TaxCalculator.GetTaxRate(request.RegionCode), totalPrice)
        });
    })
    .Produces<ApiResponse<TaxCalculationResponse>>()
    .Produces<ApiResponse<TaxCalculationResponse>>(StatusCodes.Status400BadRequest)
    .WithName("CalculateTax");

api.Run();