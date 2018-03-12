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
using SimulationUtility.ViewModels;

namespace SimulationUtility.Controls
{
    /// <summary>
    /// Interaction logic for ChunkControl.xaml
    /// </summary>
    public partial class ChunkControl : UserControl
    {
        public ChunkControl(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            var vm = new ChunkControlViewModel(mainPageViewModel) {ControlReference = this};
            this.DataContext = vm;
        }

        public ChunkControl(ChunkControlViewModel vm)
        {
            InitializeComponent();
            vm.ControlReference = this;
            this.DataContext = vm;
        }


        public List<TransactionEvent> GetEvents()
        {
            var vm = (ChunkControlViewModel) DataContext;

            return vm.GetEvents();
        }
    }
}
