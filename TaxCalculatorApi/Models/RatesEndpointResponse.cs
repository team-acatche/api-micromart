namespace TaxCalculatorApi.Models;

/// <summary>
/// A response format for the <c>/rate/{regionCode}</c> endpoint.
/// </summary>
/// <param name="regionCode">the region code wherein the rate applies to.</param>
/// <param name="taxRate">the sales tax rate for the specified region code.</param>
public record RatesEndpointResponse(string regionCode, decimal taxRate);