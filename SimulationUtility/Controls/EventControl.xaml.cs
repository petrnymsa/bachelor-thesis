using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BachelorThesis.Bussiness.DataModels;

namespace SimulationUtility.Controls
{
    /// <summary>
    /// Interaction logic for EventControl.xaml
    /// </summary>
    public partial class EventControl : UserControl
    {
        public EventControl(ChunkControlViewModel chunkControlViewModel)
        {
            InitializeComponent();
            
            var vm = new EventControlViewModel(chunkControlViewModel) { ControlReference = this};

            this.DataContext = vm;
        }

        public EventControl(EventControlViewModel vm)
        {
            InitializeComponent();

            vm.ControlReference = this;
            this.DataContext = vm;
        }

        public TransactionEvent GetEvent()
        {
            var vm = (EventControlViewModel) DataContext;

            return vm.GetTransactionEvent();
        }
    }
}
