namespace DiscountEngineApi.Models;

public record DiscountEngineResponse
{
    public required decimal OriginalPrice { get; init; }
    public required string DiscountApplied { get; init; }
    public required decimal FinalPrice { get; init; }
}