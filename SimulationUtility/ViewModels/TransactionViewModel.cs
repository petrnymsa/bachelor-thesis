using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business.DataModels;

namespace SimulationUtility.ViewModels
{
    public class TransactionViewModel
    {
        public TransactionInstance Instance { get; set; }
        public string Name { get; set; }

        public TransactionViewModel(TransactionInstance instance, string name)
        {
            Instance = instance;
            Name = name;
        }
    }
}
