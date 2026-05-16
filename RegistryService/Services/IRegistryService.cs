using RegistryService.Models;

namespace RegistryService.Services;

public interface IRegistryService
{
    List<RegistryComponent> GetComponents();
    RegistryComponent? GetComponentWithName(string name);
}