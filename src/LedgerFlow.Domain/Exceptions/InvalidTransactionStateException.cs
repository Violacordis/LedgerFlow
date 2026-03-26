namespace LedgerFlow.Domain.Exceptions;

public class InvalidTransactionStateException : Exception
{
    public InvalidTransactionStateException(string reference, string from, string to)
        : base($"Transaction {reference} cannot transition from {from} to {to}")
    {
    }
}
