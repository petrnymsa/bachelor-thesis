using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Bussiness.DataModels;
using BachelorThesis.Bussiness.Parsers;
using SimulationUtility.Common;
using SimulationUtility.ViewModels;

namespace SimulationUtility.Controls
{
    public class EventControlViewModel : BaseViewModel
    {
        private readonly ChunkControlViewModel chunkControlViewModel;

        public EventControl ControlReference { get; set; }

        public Command RemoveMeCommand { get; set; }

        public List<TransactionCompletion> TransactionCompletions { get; set; }
        public TransactionCompletion SelectedCompletion { get; set; }

        public List<TransactionEventType> TransactionEventTypes { get; set; }

        public TransactionEventType SelectedEventType { get; set; }

        public ObservableCollection<Actor> Actors { get; set; }

        public Actor SelectedActor { get; set; }

        public string CreationTime { get; set; }

        public ObservableCollection<TransactionInstance> TransactionInstances { get; set; }

        public TransactionInstance SelectedTransactionInstance { get; set; }


        public EventControlViewModel(ChunkControlViewModel chunkControlViewModel)
        {
            this.chunkControlViewModel = chunkControlViewModel;

            RemoveMeCommand = new Command(RemoveMeCommandExecute);

            TransactionEventTypes = new List<TransactionEventType>(Enum.GetValues(typeof(TransactionEventType)).Cast<TransactionEventType>());
            TransactionCompletions = new List<TransactionCompletion>(Enum.GetValues(typeof(TransactionCompletion)).Cast<TransactionCompletion>());

            Actors = new ObservableCollection<Actor>();

            foreach (var actor in MainPageViewModel.ParserResult.Actors)
                Actors.Add(actor);

            TransactionInstances = new ObservableCollection<TransactionInstance>();

            foreach (var instance in MainPageViewModel.ParserResult.ProcessInstance.GetTransactions())
            {
                TransactionInstances.Add(instance);
                TreeStructureHelper.Traverse(instance, TransactionInstances, (i, list) => list.Add(i));

            }

            CreationTime = "01-02-2018 15:34:23";
        }

        private void RemoveMeCommandExecute(object obj)
        {
            chunkControlViewModel.RemoveEvent(ControlReference);
        }

        public TransactionEvent GetTransactionEvent()
        {
            return new CompletionChangedTransactionEvent(SelectedTransactionInstance.Id, SelectedActor.Id,
                DateTime.ParseExact(CreationTime, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture), SelectedCompletion);
        }
    }
}
