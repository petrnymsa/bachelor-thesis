using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    interface IProcessDefinition
    {
        ProcessKind GetDefinition();
        //   List<TransactionKind> GetTransactionDefinitions();
    }
}