using System.Collections.Generic;
using System.Collections.ObjectModel;
using BachelorThesis.Business.DataModels;
using SimulationUtility.Common;
using SimulationUtility.Controls;

namespace SimulationUtility.ViewModels
{
    public class ChunkControlViewModel : BaseViewModel
    {
        private readonly MainPageViewModel mainPageViewModel;
        public ObservableCollection<EventControl> Events { get; set; }

        public Command AddEventCommand { get; set; }
        public Command RemoveMeCommand { get; set; }

        public ChunkControlViewModel()
        {
            
        }

        public ChunkControlViewModel(ViewModels.MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            Events = new ObservableCollection<EventControl>();

            AddEventCommand = new Command(AddEventCommandExecute);
            RemoveMeCommand = new Command(RemoveMeCommandExecute);
        }

        private void RemoveMeCommandExecute(object obj)
        {
            mainPageViewModel.RemoveChunk(ControlReference);
        }

        public ChunkControl ControlReference { get; set; }

        private void AddEventCommandExecute(object obj)
        {
            Events.Add(new EventControl(this));
        }

        public void RemoveEvent(EventControl eventControl)
        {
            Events.Remove(eventControl);
        }

        public List<TransactionEvent> GetEvents()
        {
            var list = new List<TransactionEvent>();

            foreach (var control in Events)
            {
                var ev = control.GetEvent();

                list.Add(ev);
            }

            return list;
        }
    }
}
