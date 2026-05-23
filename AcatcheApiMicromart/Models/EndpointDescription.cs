namespace AcatcheApiMicromart.Models;

public class EndpointDescription
{
    public string Path { get; set; } = "";
    public string Method { get; set; } = "";
    public string? SampleRequest { get; set; }
    public string? SampleResponse { get; set; }
}