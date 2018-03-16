using System.Windows.Controls;
using BachelorThesis.Business.DataModels;
using SimulationUtility.ViewModels;

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
