namespace LedgerFlow.Domain.Exceptions;

public class AccountSuspendedException : Exception
{
    public AccountSuspendedException(string accountNumber)
        : base($"Account {accountNumber} is suspended and cannot be used for transactions")
    {
    }
}
