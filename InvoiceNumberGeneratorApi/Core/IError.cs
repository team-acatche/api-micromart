namespace InvoiceNumberGeneratorApi.Core;

/// <summary>
/// A default infallible Error value interface for more effective error handling.
/// </summary>
public interface IError
{
    string Message { get; }
}