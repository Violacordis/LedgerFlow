namespace LedgerFlow.Domain.Enums;

public enum TransactionStatus
{
    Pending,
    Processing,
    RiskCheck,
    Approved,
    Flagged,
    Declined,
    Settled
}
