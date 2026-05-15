namespace DiscountEngineApi.Services;

public class BaseDiscountRules
{
    public class Save10 : IDiscountRule
    {
        public decimal Apply(decimal price)
        {
            return price - (price * 0.10m);
        }
    }
    
    public class Flat50 : IDiscountRule
    {
        public decimal Apply(decimal price)
        {
            return price - 50;
        }
    }
    
    public class HalfOff : IDiscountRule
    {
        public decimal Apply(decimal price)
        {
            return price - (price * 0.50m);
        }
    }
}