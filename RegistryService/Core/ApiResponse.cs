namespace RegistryService.Core;

public record ApiResponse<T>
{
    public required bool Success { get; init; }
    public required string ComponentName { get; init; }
    public T? Result { get; init; } = default(T);
    public IError? Error { get; init; } = null;
}