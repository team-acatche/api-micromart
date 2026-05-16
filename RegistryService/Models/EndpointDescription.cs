namespace RegistryService.Models;

public record EndpointDescription
{
    public required string Path { get; init; }
    public required string Method { get; init; }
    public required string SampleRequest { get; init; }
    public required string SampleResponse { get; init; }
};