namespace DiscountEngineApi.Models;

public record DiscountEngineRequest(decimal OriginalPrice, string DiscountCode);