namespace DiscountEngineApi.Services;

public interface IDiscountResolver
{
    bool IsValidDiscount(string code);
    decimal ResolveDiscount(decimal price, string code);
}