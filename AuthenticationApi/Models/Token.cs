namespace AuthenticationApi.Models;

public record Token(string TokenString, DateTime ExpiresAt)
{
    public bool IsExpired() => DateTime.Now > ExpiresAt;
}