using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using TaxCalculatorApi.Services.Helpers;

namespace TaxCalculatorApi.Services;

/// <summary>
/// The service responsible for calculating the total price with tax.
/// </summary>
public class TaxCalculator : ITaxCalculator
{
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
        { RegionCode.Sg, 0.09m },
        { RegionCode.UsCa, 0.0725m },
        { RegionCode.UsHi, 0.045m },
    };

    public static IEnumerable<string> SupportedRegions =>
        Enum.GetNames<RegionCode>().Select(JsonNamingPolicy.KebabCaseUpper.ConvertName);

    private static readonly JsonSerializerOptions DeserializationOptions = new()
    {
        Converters = { new TaxCodeEnumConverter() }
    };

    /// <inheritdoc/>
    public IEnumerable<string> GetRegions() => SupportedRegions;

    /// <inheritdoc/>
    public decimal GetTaxRate(OneOf<RegionCode, string> regionCode) => regionCode.Match(
        code => TaxIndex[code],
        codeString => TaxIndex[JsonSerializer.Deserialize<RegionCode>($"\"{codeString}\"", DeserializationOptions)]
    );

    /// <inheritdoc/>
    public decimal Calculate(decimal price, OneOf<RegionCode, string> region) => price + (price * GetTaxRate(region));
}