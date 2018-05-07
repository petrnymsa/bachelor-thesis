using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using BachelorThesis.Business;
using Xamarin.Forms;

namespace BachelorThesis.Views
{
    public class SimulationCaseViewModel : BindableBase
    {
        public string Name { get; set; }
        public string CaseName { get; set; }
        public string Description { get; set; }

        public SimulationCaseViewModel(string name, string caseName, string description)
        {
            Name = name;
            CaseName = caseName;
            Description = description;
        }
    }

    public class SimulationCasesPageViewModel : BindableBase
    {
        private readonly INavigation navigation;
        private SimulationCaseViewModel selectedCase;
        public ObservableCollection<SimulationCaseViewModel> Cases { get; set; }

        public SimulationCaseViewModel SelectedCase
        {
            get => selectedCase;
            set { selectedCase = value; OnPropertyChanged(nameof(SelectedCase)); Navigate(); }
        }

        //  public ICommand NavigateCommand { get; set; }

        public SimulationCasesPageViewModel(INavigation navigation)
        {
         
            this.navigation = navigation;
          //  NavigateCommand = new Command<object>(Navigate, o => SelectedCase != null);

            Cases = new ObservableCollection<SimulationCaseViewModel>()
            {
                new SimulationCaseViewModel("Happy Path", SimulationCases.Case01, "A straightforward scenario with 'happy' path. The contract is requested and successfully accepted. Later the car is picked up and then returned without complications."),
                new SimulationCaseViewModel("Declined Contract", SimulationCases.Case02, "A short scenario, when contract is declined and costumer decided to leave."),
                new SimulationCaseViewModel("Penalty Payment", SimulationCases.Case03, "Most complex scenario. Firstly, contract is declined, but then successfully signed. When customer wanted to drop off the car, the penalty payment was charged.")
            };
        }

        

        private async void Navigate()
        {
            if (SelectedCase == null) return;


            await navigation.PushAsync(new ProcessVisualisationPage(SelectedCase));
            SelectedCase = null;
        }
    }
}
