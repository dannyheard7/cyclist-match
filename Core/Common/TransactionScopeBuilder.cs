using System.Transactions;

namespace Common;

public static class TransactionScopeBuilder
{
    public static TransactionScope CreateReadCommitted(TransactionScopeAsyncFlowOption asyncFlowOption)
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };

        return new TransactionScope(TransactionScopeOption.Required, options, asyncFlowOption);
    }
}