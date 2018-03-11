using System.Collections.Generic;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    interface IProcessEventDefinitions
    {
        List<TransactionEventKind> GetDefinitions();
    }
}