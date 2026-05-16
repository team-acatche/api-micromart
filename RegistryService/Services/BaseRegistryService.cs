using RegistryService.Models;

namespace RegistryService.Services;

public class BaseRegistryService : IRegistryService
{
    public required List<RegistryComponent> Components { get; init; }
    
    public List<RegistryComponent> GetComponents() => Components;

    public RegistryComponent? GetComponentWithName(string name) =>
        Components.Find(component => component.ComponentName.Equals(name));
}