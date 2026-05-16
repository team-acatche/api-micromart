namespace TaxCalculatorApi.Models;

/// <summary>
/// A standardized response body for the Tax Calculator API.
/// </summary>
public record TaxCalculationResponse
{
    /// <summary>
    /// The initial price pre-tax.
    /// </summary>
    public required decimal InitialPrice { get; init; }
    
    /// <summary>
    /// The region code whose tax rate has been applied to the initial price.
    /// </summary>
    public required string RegionCode { get; init; }
    
    /// <summary>
    /// The tax rate from the selected region code.
    /// </summary>
    public required decimal TaxRate { get; init; }
    
    /// <summary>
    /// The final price post-tax.
    /// </summary>
    public required decimal TotalPrice { get; init; }
};