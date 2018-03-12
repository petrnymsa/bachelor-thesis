using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BachelorThesis.Bussiness.DataModels;
using BachelorThesis.Bussiness.Parsers;
using BachelorThesis.Bussiness.Simulation;
using Microsoft.Win32;
using SimulationUtility.Common;
using SimulationUtility.Controls;

namespace SimulationUtility.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private SimulationCaseParser simulationCaseParser;
        private string xmlPath;


        public ObservableCollection<ChunkControl> ChunkControls { get; set; }

        public Command AddChunk { get; set; }
        public Command SaveCommand { get; set; }

        public Command LoadCommand { get; set; }

        public static SimulationCaseParserResult ParserResult { get; set; }

        public string SimulationName { get; set; }

        public MainPageViewModel()
        {
            simulationCaseParser = new SimulationCaseParser();
            ChunkControls = new ObservableCollection<ChunkControl>();

            AddChunk = new Command(AddChunkExecute, obj => ParserResult != null );
            LoadCommand = new Command(LoadCommandExecute);

            SaveCommand = new Command(SaveCommandExecute, obj => ParserResult != null);
        }

        private void SaveCommandExecute(object obj)
        {
            ParserResult.Chunks.Clear();

            foreach (var chunkControl in ChunkControls)
            {
                var chunk = new SimulationChunk();
                var events = chunkControl.GetEvents();
                foreach (var e in events)
                {
                    chunk.AddStep(e);
                }

                ParserResult.Chunks.Add(chunk);
            }

            ParserResult.Name = SimulationName;
            var doc = new SimulationCaseParser().Create(ParserResult);


            var saveDialog = new SaveFileDialog
            {
                DefaultExt = ".xml",
                Filter = "XML Files (.xml)|*.xml"
            };
            if (saveDialog.ShowDialog() == true)
            {
                doc.Save(saveDialog.FileName);

                MessageBox.Show("Oki doki, file saved");
            }
        }

        private void LoadCommandExecute(object obj)
        {
         
            var dialog = new OpenFileDialog();
           
            if (dialog.ShowDialog() == true)
            {
                ChunkControls.Clear();
                xmlPath = dialog.FileName;
                ParserResult = simulationCaseParser.Parse(xmlPath);

                SimulationName = ParserResult.Name;

                foreach (var chunk in ParserResult.Chunks)
                {
                    var chunkVm = new ChunkControlViewModel(this);

                    foreach (var tEvent in chunk.GetEvents())
                    {
                        var eventVm = new EventControlViewModel(chunkVm);
                        eventVm.CreationTime = tEvent.Created.ToString(XmlParsersConfig.DateTimeFormat);
                        eventVm.SelectedActor = ParserResult.Actors.Find(x => x.Id == tEvent.RaisedByActorId);

                        eventVm.SelectedCompletion = ((CompletionChangedTransactionEvent) tEvent).Completion;
                        eventVm.SelectedTransactionInstance =
                            ParserResult.ProcessInstance.GetTransactionById(tEvent.Id);

                        chunkVm.Events.Add(new EventControl(eventVm));
                    }

                    ChunkControls.Add(new ChunkControl(chunkVm));

                }
            }
        }

        private void AddChunkExecute(object obj)
        {
            ChunkControls.Add(new ChunkControl(this));
        }

        public void RemoveChunk(ChunkControl chunkControl)
        {
            ChunkControls.Remove(chunkControl);
        }
    }
}
