namespace LedgerFlow.Domain.Entities;

public class FraudAlert
{
    public Guid Id { get; private set; }
    public Guid TransactionId { get; private set; }
    public string RuleTriggered { get; private set; } = string.Empty;
    public int ScoreImpact { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Transaction Transaction { get; private set; } = null!;

    private FraudAlert() { }

    public static FraudAlert Create(Guid transactionId, string ruleTriggered, int scoreImpact)
    {
        return new FraudAlert
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            RuleTriggered = ruleTriggered,
            ScoreImpact = scoreImpact,
            CreatedAt = DateTime.UtcNow
        };
    }
}
