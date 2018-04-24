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
                new SimulationCaseViewModel("Happy Path", SimulationCases.Case01, "..."),
                new SimulationCaseViewModel("Declined Contract", SimulationCases.Case02, "..."),
            };
        }

        

        private async void Navigate()
        {
            if (SelectedCase != null)
            {
                await navigation.PushAsync(new ProcessVisualisationPage(SelectedCase));
            }
        }
    }
}
