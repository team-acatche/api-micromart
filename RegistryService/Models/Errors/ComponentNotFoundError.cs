using RegistryService.Core;

namespace RegistryService.Models.Errors;

public class ComponentNotFoundError(string componentName) : IError
{
    public string Message => $"Component named `{componentName}` not found in the registry";
}