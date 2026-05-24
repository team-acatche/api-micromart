using AuthenticationApi.Core;

namespace AuthenticationApi.Models.Errors;

public class EmptyFieldError(string[] emptyFieldNames) : IError
{
    public string Message { get; } = $"The following fields are empty: {string.Join(", ", emptyFieldNames)}";
}