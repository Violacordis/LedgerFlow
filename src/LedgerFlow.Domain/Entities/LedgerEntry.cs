namespace LedgerFlow.Domain.Entities;

public class LedgerEntry
{
    public Guid Id { get; private set; }
    public Guid TransactionId { get; private set; }
    public Guid AccountId { get; private set; }
    public decimal Debit { get; private set; }
    public decimal Credit { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Transaction Transaction { get; private set; } = null!;
    public Account Account { get; private set; } = null!;

    private LedgerEntry() { }

    public static LedgerEntry CreateDebit(Guid transactionId, Guid accountId, decimal amount)
    {
        return new LedgerEntry
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            AccountId = accountId,
            Debit = amount,
            Credit = 0,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static LedgerEntry CreateCredit(Guid transactionId, Guid accountId, decimal amount)
    {
        return new LedgerEntry
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            AccountId = accountId,
            Debit = 0,
            Credit = amount,
            CreatedAt = DateTime.UtcNow
        };
    }
}
