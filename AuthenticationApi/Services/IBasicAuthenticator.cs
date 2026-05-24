using AuthenticationApi.Models;

namespace AuthenticationApi.Services;

public interface IBasicAuthenticator
{
    Token? Login(string username, string password);
    User? GetUserFromToken(string token);
    User? Register(string username, string password);
}