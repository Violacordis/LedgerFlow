namespace LedgerFlow.Domain.Exceptions;

public class AccountSuspendedException : Exception
{
    public AccountSuspendedException(string accountNumber, string status)
        : base($"Account {accountNumber} is {status} and cannot be used for transactions")
    {
    }
}
