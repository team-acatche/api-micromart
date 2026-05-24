namespace RegistryService.Models;

public record RegistryComponent
{
    public required string ComponentName { get; init; }
    public required string Description { get; init; }
    public required string Version { get; init; }
    public required string BaseUrl { get; init; }
    public required string InternalUrl { get; init; }   // internal, for Blazor Execute()
    public required List<EndpointDescription> Endpoints { get; init; }
}