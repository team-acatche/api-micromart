using OneOf;

namespace TaxCalculatorApi.Services;

public interface ITaxCalculator
{
    /// <summary>
    /// Obtain all of this calculator's supported regions.
    /// </summary>
    public IEnumerable<string> GetRegions();
    
    /// <summary>
    /// Obtain a region's sales tax rate.
    /// </summary>
    /// <param name="regionCode">the region code specifying the region.</param>
    /// <returns>the region's sales tax rate.</returns>
    public decimal GetTaxRate(OneOf<TaxCalculator.RegionCode, string> regionCode);

    /// <summary>
    /// Calculate a total price based on the initial price and a region code.
    /// </summary>
    /// <param name="price">the initial price.</param>
    /// <param name="regionCode">the region code specifying the region to calculate the price in.</param>
    /// <returns>the total price with tax added.</returns>
    public decimal Calculate(decimal price, OneOf<TaxCalculator.RegionCode, string> regionCode);
}