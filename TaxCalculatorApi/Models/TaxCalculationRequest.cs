namespace TaxCalculatorApi.Models;

/// <summary>
/// The standardized request body for the Tax Calculator API.
/// </summary>
/// <param name="Price">the initial price.</param>
/// <param name="RegionCode">the region code wherein the sales tax will be calculated.</param>
public record TaxCalculationRequest(decimal Price, TaxCalculator.RegionCode RegionCode);