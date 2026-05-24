using AuthenticationApi.Core;

namespace AuthenticationApi.Models.Errors;

public class InvalidCredentialsError : IError
{
    public string Message => "Invalid username or password";
}