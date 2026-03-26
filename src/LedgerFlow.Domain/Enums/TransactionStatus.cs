namespace LedgerFlow.Domain.Enums;

public enum TransactionStatus
{
    Received,
    Processing,
    RiskCheck,
    Approved,
    Flagged,
    Declined,
    Settled
}
