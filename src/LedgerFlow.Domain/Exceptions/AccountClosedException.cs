namespace LedgerFlow.Domain.Exceptions;

public class AccountClosedException : Exception
{
    public AccountClosedException(string accountNumber)
        : base($"Account {accountNumber} is closed and cannot be used for transactions")
    {
    }
}
