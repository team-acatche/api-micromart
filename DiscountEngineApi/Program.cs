using DiscountEngineApi;
using DiscountEngineApi.Core;
using DiscountEngineApi.Models;
using DiscountEngineApi.Models.Errors;
using DiscountEngineApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTransient<IDiscountResolver, BaseDiscountResolver>();

var api = builder.Build();

// Configure the HTTP request pipeline.
if (api.Environment.IsDevelopment())
{
    api.MapOpenApi();
    api.MapScalarApiReference();
}

api.UseHttpsRedirection();

var app = api.MapGroup("api/discount");

app.MapPost("/apply", IResult (IDiscountResolver discountResolver, DiscountEngineRequest request) =>
    {
        if (request.OriginalPrice < 0)
        {
            return TypedResults.BadRequest(new ApiResponse<DiscountEngineResponse>
            {
                ComponentName = "DiscountEngine",
                Success = false,
                Error = new InitialPriceBelowZeroError(request.OriginalPrice),
            });
        }

        if (!discountResolver.IsValidDiscount(request.DiscountCode))
        {
            return TypedResults.BadRequest(new ApiResponse<DiscountEngineResponse>
            {
                ComponentName = "DiscountEngine",
                Success = false,
                Error = new InvalidDiscountError(request.DiscountCode),
            });
        }

        return TypedResults.Ok(new ApiResponse<DiscountEngineResponse>
        {
            ComponentName = "DiscountEngine",
            Success = true,
            Result = new DiscountEngineResponse
            {
                DiscountApplied = request.DiscountCode,
                OriginalPrice = request.OriginalPrice,
                FinalPrice = discountResolver.ResolveDiscount(request.OriginalPrice, request.DiscountCode),
            },
        });
    })
    .Produces<ApiResponse<DiscountEngineResponse>>()
    .Produces<ApiResponse<DiscountEngineResponse>>(StatusCodes.Status400BadRequest)
    .WithName("ApplyDiscount");

api.Run();