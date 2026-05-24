namespace AuthenticationApi.Models;

public record User(int UserId, string Username, string Salt, string PasswordHash);