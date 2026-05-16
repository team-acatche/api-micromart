using System.ComponentModel.DataAnnotations;
using TaxCalculatorApi.Services;

namespace TaxCalculatorApi.Models;

/// <summary>
/// The standardized request body for the Tax Calculator API.
/// </summary>
/// <param name="Price">the initial price.</param>
/// <param name="RegionCode">the region code wherein the sales tax will be calculated.</param>
public record TaxCalculationRequest(
    [Range(0.0, double.MaxValue, ErrorMessage = "Price must be equal to or greater than zero")]
    decimal Price,
    [EnumDataType(typeof(TaxCalculator.RegionCode))]
    string RegionCode);