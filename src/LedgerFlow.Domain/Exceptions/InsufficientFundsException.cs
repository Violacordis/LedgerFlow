namespace LedgerFlow.Domain.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string accountNumber, decimal balance, decimal amount)
        : base($"Account {accountNumber} has insufficient funds. Balance: {balance:N2}, Required: {amount:N2}")
    {
    }
}
