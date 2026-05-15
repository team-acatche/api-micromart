using DiscountEngineApi.Core;

namespace DiscountEngineApi.Models.Errors;

public class InvalidDiscountError(string discountCode) : IError
{
    public string Message => $"The discount code `{discountCode}` is invalid.";
}