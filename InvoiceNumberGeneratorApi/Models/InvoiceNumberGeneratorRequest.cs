namespace InvoiceNumberGeneratorApi.Models;

public record InvoiceNumberGeneratorRequest
{
    public required string Prefix { get; init; }
    public required string ClientCode { get; init; }
};