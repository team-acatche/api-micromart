using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaxCalculatorApi;

/// <summary>
/// The service responsible for calculating the total price with tax.
/// </summary>
public abstract class TaxCalculator
{
    private class TaxCodeEnumConverter() : JsonStringEnumConverter(JsonNamingPolicy.KebabCaseUpper);
    
    /// <summary>
    /// The enum containing all currently supported regions and their region codes.
    /// </summary>
    [JsonConverter(typeof(TaxCodeEnumConverter))]
    public enum RegionCode
    {
        PhNcr,
        PhCeb,
        Sg,
        UsCa,
        UsHi,
    }
    
    /// <summary>
    /// A dictionary of region codes and their sales tax rates.
    /// </summary>
    public static readonly Dictionary<RegionCode, decimal> TaxIndex = new()
    {
        { RegionCode.PhNcr, 0.12m },
        { RegionCode.PhCeb, 0.12m },
        { RegionCode.Sg, 0.9m },
        { RegionCode.UsCa, 0.725m },
        { RegionCode.UsHi, 0.45m },
    };

    /// <summary>
    /// Obtain a region's sales tax rate.
    /// </summary>
    /// <param name="regionCode">the region code specifying the region.</param>
    /// <returns>the region's sales tax rate.</returns>
    public static decimal GetTaxRate(RegionCode regionCode)
    {
        return TaxIndex[regionCode];
    }
    
    /// <summary>
    /// Calculate a total price based on the initial price and a region code.
    /// </summary>
    /// <param name="price">the initial price.</param>
    /// <param name="region">the region code specifying the region to calculate the price in.</param>
    /// <returns>the total price with tax added.</returns>
    public static decimal Calculate(decimal price, RegionCode region)
    {
        return price + (price * TaxIndex[region]);
    }
}