using System.Security.Cryptography;
using System.Text;
using AuthenticationApi.Models;

namespace AuthenticationApi.Services;

public class Sha256BasicAuthenticator : IBasicAuthenticator
{
    private const int SaltLength = 7;
    private const int TokenLength = 32;

    private static readonly int TokenExpirationMinutes =
        Convert.ToInt32(Environment.GetEnvironmentVariable("TOKEN_EXPIRATION_MINUTES") ?? "5");

    private readonly SHA256 _hashFunction = SHA256.Create();
    private readonly Random _rng = new();

    private readonly List<User> _users =
    [
        new(0, "admin", "sample_", "uo+L7ULDISZOkFUw3CH0a4vcbW9Ee/X4bgLcex+FWec="),
    ];

    private readonly Dictionary<int, Token> _validTokens = new();

    public Token? Login(string username, string password)
    {
        var user = _users.FirstOrDefault(x => x.Username == username);

        if (user == null) return null;
        if (user.PasswordHash != HashPassword(password, user.Salt)) return null;

        if (_validTokens.TryGetValue(user.UserId, out var loginToken)) return loginToken!;

        var tokenString = GenerateRandomString(TokenLength);
        var expirationDate = DateTime.Now + TimeSpan.FromMinutes(TokenExpirationMinutes);
        var token = new Token(tokenString, expirationDate);
        _validTokens.Add(user.UserId, token);
        return token;
    }

    public User? GetUserFromToken(string token)
    {
        var savedToken = _validTokens.FirstOrDefault(x => x.Value.TokenString == token);
        if (savedToken.Value.IsExpired()) _validTokens.Remove(savedToken.Key);
        return _users.SingleOrDefault(x => x.UserId == savedToken.Key);
    }

    public User? Register(string username, string password)
    {
        if (_users.Any(user => user.Username == username)) return null;

        var salt = GenerateRandomString(SaltLength);
        var hashedPassword = HashPassword(password, salt);

        var newUser = new User(_users.Count, username, salt, hashedPassword);
        _users.Add(newUser);
        return newUser;
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_rng.Next(s.Length)])
            .ToArray());
    }

    private string HashPassword(string password, string salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Encoding.UTF8.GetBytes(salt);

        var saltedPassword = new byte[passwordBytes.Length + saltBytes.Length];
        // copy the bytes of the salt and the password into saltedPassword (salt + password)
        Buffer.BlockCopy(saltBytes, 0, saltedPassword, 0, saltBytes.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, saltBytes.Length, passwordBytes.Length);

        var hashBytes = _hashFunction.ComputeHash(saltedPassword);
        return Convert.ToBase64String(hashBytes);
    }
}