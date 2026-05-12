using TaxCalculatorApi.Core;

namespace TaxCalculatorApi.Models.Errors;

public class InitialPriceBelowZeroError(decimal price) : IError
{
    public string Message => $"Initial price ({price}) cannot be negative";
}