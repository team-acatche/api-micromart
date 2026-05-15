namespace DiscountEngineApi.Services;

public interface IDiscountRule
{
    decimal Apply(decimal price);
}