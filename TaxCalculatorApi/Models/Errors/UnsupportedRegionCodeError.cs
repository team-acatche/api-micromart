using TaxCalculatorApi.Core;

namespace TaxCalculatorApi.Models.Errors;

public class UnsupportedRegionCodeError(string regionCode) : IError
{
    public string Message =>
        $"The region code '{regionCode}' is not currently supported. Supported region codes are: {string.Join(", ", TaxCalculator.SupportedRegions)}.";
}