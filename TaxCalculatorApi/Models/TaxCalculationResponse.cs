namespace TaxCalculatorApi.Models;

/// <summary>
/// A standardized response body for the Tax Calculator API.
/// </summary>
/// <param name="Details">the details of the request.</param>
/// <param name="TaxRate">the tax rate for the specified region.</param>
/// <param name="TotalPrice">the total calculated price.</param>
public record TaxCalculationResponse(TaxCalculationRequest Details, decimal TaxRate, decimal TotalPrice);