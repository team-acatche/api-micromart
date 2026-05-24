using AuthenticationApi.Core;

namespace AuthenticationApi.Models.Errors;

public class UsernameAlreadyTakenError : IError
{
    public string Message => "Username already taken";
    
}