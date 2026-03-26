using LedgerFlow.Domain.Enums;

namespace LedgerFlow.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public Guid AccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Account Account { get; private set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

    private User() { }

    public static User Create(string email, string passwordHash, Guid accountId, UserRole role = UserRole.Customer)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            AccountId = accountId,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
    }
}
