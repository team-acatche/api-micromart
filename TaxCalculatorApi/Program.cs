using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using TaxCalculatorApi.Core;
using TaxCalculatorApi.Models;
using TaxCalculatorApi.Models.Errors;
using TaxCalculatorApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ITaxCalculator, TaxCalculator>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var api = builder.Build();

// Configure the HTTP request pipeline.
if (api.Environment.IsDevelopment())
{
    api.MapOpenApi();
    api.MapScalarApiReference();
}

api.UseCors();
api.UseHttpsRedirection();

var app = api.MapGroup("api/tax");

app.MapGet("/regions", IResult (ITaxCalculator calculator) => TypedResults.Ok(new ApiResponse<List<string>>()
    {
        ComponentName = "TaxCalculator",
        Success = true,
        Result = calculator.GetRegions().ToList()
    }))
    .Produces<ApiResponse<List<string>>>()
    .WithName("GetRegions");

app.MapGet("/rate/{regionCode}", IResult (ITaxCalculator calculator, string regionCode) =>
{
    if (!calculator.GetRegions().Contains(regionCode))
        return TypedResults.BadRequest(new ApiResponse<RatesEndpointResponse?>()
        {
            ComponentName = "TaxCalculator",
            Success = false,
            Error = new UnsupportedRegionCodeError(regionCode),
        });

    return TypedResults.Ok(new ApiResponse<RatesEndpointResponse>()
    {
        ComponentName = "TaxCalculator",
        Success = true,
        Result = new RatesEndpointResponse(regionCode, calculator.GetTaxRate(regionCode))
    });
})
    .Produces<ApiResponse<RatesEndpointResponse>>()
    .Produces<ApiResponse<RatesEndpointResponse?>>(StatusCodes.Status400BadRequest)
    .WithName("GetTaxRate");

app.MapPost("/calculate", IResult (ITaxCalculator calculator, TaxCalculationRequest request) =>
    {
        if (request.Price < 0)
            return TypedResults.BadRequest(new ApiResponse<TaxCalculationResponse?>()
            {
                ComponentName = "TaxCalculator",
                Success = false,
                Error = new InitialPriceBelowZeroError(request.Price),
            });

        if (!calculator.GetRegions().Contains(request.RegionCode))
            return TypedResults.BadRequest(new ApiResponse<TaxCalculationResponse?>()
            {
                ComponentName = "TaxCalculator",
                Success = false,
                Error = new UnsupportedRegionCodeError(request.RegionCode),
            });

        var response = new ApiResponse<TaxCalculationResponse>()
        {
            ComponentName = "TaxCalculator",
            Success = true,
            Result = new TaxCalculationResponse
            {
                InitialPrice = request.Price,
                RegionCode = request.RegionCode,
                TaxRate = calculator.GetTaxRate(request.RegionCode),
                TotalPrice = calculator.Calculate(request.Price, request.RegionCode),
            }
        };
        return TypedResults.Ok(response);
    })
    .Produces<ApiResponse<TaxCalculationResponse>>()
    .Produces<ApiResponse<TaxCalculationResponse?>>(StatusCodes.Status400BadRequest)
    .WithName("CalculateTax");

api.Run();