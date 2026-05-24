namespace AuthenticationApi.Models;

public record LoginResponse(string Token, int UserId, string Username);