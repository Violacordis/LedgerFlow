using LedgerFlow.Domain.Enums;
using LedgerFlow.Domain.Exceptions;

namespace LedgerFlow.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public AccountStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Account() { }

    public static Account Create(string accountNumber, string name)
    {
        return new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = accountNumber,
            Name = name,
            Balance = 0,
            Status = AccountStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
    }

    private void GuardAccountActive()
    {
        if (Status == AccountStatus.Suspended)
            throw new AccountSuspendedException(AccountNumber);
        if (Status == AccountStatus.Closed)
            throw new AccountClosedException(AccountNumber);
    }

    public void Debit(decimal amount)
    {
        GuardAccountActive();

        if (amount <= 0)
            throw new ArgumentException("Debit amount must be greater than zero.");

        if (Balance < amount)
            throw new InsufficientFundsException(AccountNumber, Balance, amount);

        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        GuardAccountActive();

        if (amount <= 0)
            throw new ArgumentException("Credit amount must be greater than zero.");

        Balance += amount;
    }

    public void Suspend() => Status = AccountStatus.Suspended;
    public void Activate() => Status = AccountStatus.Active;
    public void Close() => Status = AccountStatus.Closed;
}
