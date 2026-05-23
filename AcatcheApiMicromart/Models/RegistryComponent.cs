namespace AcatcheApiMicromart.Models;

public class RegistryComponent
{
    public string ComponentName { get; set; } = "";
    public string Description { get; set; } = "";
    public string Version { get; set; } = "";
    public string BaseUrl { get; set; } = "";
    public List<EndpointDescription>? Endpoints { get; set; }
}