using InvoiceNumberGeneratorApi.Core;

namespace InvoiceNumberGeneratorApi.Models.Errors;

public class EmptyFieldError(string[] emptyFieldNames) : IError
{
    public string Message { get; } = $"The following fields are empty: {string.Join(", ", emptyFieldNames)}";
}