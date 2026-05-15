namespace DiscountEngineApi.Services;

public class BaseDiscountResolver : IDiscountResolver
{
    private readonly Dictionary<string, IDiscountRule> _rules = new()
    {
        { "save10", new BaseDiscountRules.Save10() },
        { "flat50", new BaseDiscountRules.Flat50() },
        { "halfoff", new BaseDiscountRules.HalfOff() },
    };

    public bool IsValidDiscount(string code) => _rules.ContainsKey(code.ToLower());

    public decimal ResolveDiscount(decimal price, string code) =>
        IsValidDiscount(code) ? _rules[code.ToLower()].Apply(price) : price;
}