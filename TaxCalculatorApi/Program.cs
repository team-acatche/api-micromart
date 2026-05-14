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

var app = api.MapGroup("api/tax");

app.MapPost("/calculate", IResult (TaxCalculationRequest request) =>
    {
        if (request.Price < 0)
            return TypedResults.BadRequest(new ApiResponse<TaxCalculationResponse>()
            {
                ComponentName = "TaxCalculator",
                Success = false,
                Error = new InitialPriceBelowZeroError(request.Price),
            });

        if (!TaxCalculator.SupportedRegions.Contains(request.RegionCode))
            return TypedResults.BadRequest(new ApiResponse<TaxCalculationResponse>()
            {
                ComponentName = "TaxCalculator",
                Success = false,
                Error = new UnsupportedRegionCodeError(request.RegionCode),
            });

        var regionCode = Enum.Parse<TaxCalculator.RegionCode>(request.RegionCode, ignoreCase: true);
        var totalPrice = TaxCalculator.Calculate(request.Price, regionCode);
        return TypedResults.Ok(new ApiResponse<TaxCalculationResponse>()
        {
            ComponentName = "TaxCalculator",
            Success = true,
            Result = new TaxCalculationResponse(request, TaxCalculator.GetTaxRate(regionCode), totalPrice)
        });
    })
    .Produces<ApiResponse<TaxCalculationResponse>>()
    .Produces<ApiResponse<TaxCalculationResponse>>(StatusCodes.Status400BadRequest)
    .WithName("CalculateTax");

api.Run();