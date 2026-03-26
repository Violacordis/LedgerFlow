using LedgerFlow.Domain.Enums;
using LedgerFlow.Domain.Exceptions;

namespace LedgerFlow.Domain.Entities;

public class Transaction
{
    private static readonly IReadOnlyDictionary<TransactionStatus, IReadOnlyList<TransactionStatus>> AllowedTransitions =
        new Dictionary<TransactionStatus, IReadOnlyList<TransactionStatus>>
        {
            { TransactionStatus.Pending,    [TransactionStatus.Processing] },
            { TransactionStatus.Processing, [TransactionStatus.RiskCheck] },
            { TransactionStatus.RiskCheck,  [TransactionStatus.Approved, TransactionStatus.Flagged, TransactionStatus.Declined] },
            { TransactionStatus.Approved,   [TransactionStatus.Settled] },
            { TransactionStatus.Flagged,    [TransactionStatus.Settled] }
        };

    public Guid Id { get; private set; }
    public string Reference { get; private set; } = string.Empty;
    public string IdempotencyKey { get; private set; } = string.Empty;
    public Guid SenderAccountId { get; private set; }
    public Guid ReceiverAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionStatus Status { get; private set; }
    public int RiskScore { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Account SenderAccount { get; private set; } = null!;
    public Account ReceiverAccount { get; private set; } = null!;

    private Transaction() { }

    public static Transaction Create(Guid senderAccountId, Guid receiverAccountId, decimal amount, string idempotencyKey)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Transaction amount must be greater than zero.");

        if (senderAccountId == receiverAccountId)
            throw new ArgumentException("Sender and receiver accounts must be different.", nameof(receiverAccountId));

        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("Idempotency key is required.", nameof(idempotencyKey));

        return new Transaction
        {
            Id = Guid.NewGuid(),
            Reference = GenerateReference(),
            IdempotencyKey = idempotencyKey,
            SenderAccountId = senderAccountId,
            ReceiverAccountId = receiverAccountId,
            Amount = amount,
            Status = TransactionStatus.Pending,
            RiskScore = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Advance(TransactionStatus next)
    {
        if (!AllowedTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(next))
            throw new InvalidTransactionStateException(Reference, Status.ToString(), next.ToString());

        Status = next;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApplyRiskScore(int score)
    {
        RiskScore = score;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string GenerateReference()
    {
        return $"TXN-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";
    }
}
