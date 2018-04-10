using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Parsers;
using BachelorThesis.Business.Simulation;
using Microsoft.Win32;
using SimulationUtility.Common;
using SimulationUtility.Controls;

namespace SimulationUtility.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly SimulationCaseParser simulationCaseParser;
        private string xmlPath;

        public static ProcessKind ProcessKind { get; set; } //= new ProcessKind("not loaded");
        public static ObservableCollection<ActorRole> ActorRoles { get; set; } = new ObservableCollection<ActorRole>();


        public ObservableCollection<ChunkControl> ChunkControls { get; set; }

        public Command AddChunkCommand { get; set; }
        public Command SaveSimulationCommand { get; set; }
        public Command LoadSimulationCommand { get; set; }
        public Command LoadModelCommand { get; set; }

        public static SimulationCaseParserResult ParserResult { get; set; }

        public string SimulationName { get; set; }

        public MainPageViewModel()
        {
            simulationCaseParser = new SimulationCaseParser();
            ChunkControls = new ObservableCollection<ChunkControl>();

            AddChunkCommand = new Command(AddChunkCommandExecute, obj => ParserResult != null );
            LoadSimulationCommand = new Command(LoadSimulationCommandExecute, obj => ProcessKind != null);
            LoadModelCommand = new Command(LoadModelCommandExecute);

            SaveSimulationCommand = new Command(SaveSimulationCommandExecute, obj => ParserResult != null);
        }

        private void LoadModelCommandExecute(object obj)
        {
            var dialog = new OpenFileDialog()
            {
                DefaultExt = ".xml",
                Filter = "XML Files (.xml)|*.xml"
            };
            if(dialog.ShowDialog() != true) return;
            

            var parser = new ProcessKindXmlParser();
            var result = parser.ParseDefinition(XDocument.Load(dialog.FileName));

            ProcessKind = result.ProcessKind;
            result.ActorRoles.ForEach(x => ActorRoles.Add(x));

        }

        private void SaveSimulationCommandExecute(object obj)
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
            if (saveDialog.ShowDialog() != true) return;

            doc.Save(saveDialog.FileName);
            MessageBox.Show("Oki doki, file saved");
        }

        private void LoadSimulationCommandExecute(object obj)
        {

            var dialog = new OpenFileDialog()
            {
                DefaultExt = ".xml",
                Filter = "XML Files (.xml)|*.xml"
            };

            if (dialog.ShowDialog() != true) return;

            ChunkControls.Clear();
            xmlPath = dialog.FileName;

            var xml = File.ReadAllText(xmlPath);
            ParserResult = simulationCaseParser.Parse(xml);

            SimulationName = ParserResult.Name;

            foreach (var chunk in ParserResult.Chunks)
            {
                var chunkVm = new ChunkControlViewModel(this);

                foreach (var tEvent in chunk.GetEvents())
                {
                //    var actor = ParserResult.Actors.Find(x => x.Id == tEvent.RaisedByActorId);
                 //   var role = GetActorRole(actor.ActorKindId);
                  //  var actorVm = new ActorViewModel(actor.Id,actor.FullName,role.Name, role.Id);

                    var tInstance = ParserResult.ProcessInstance.GetTransactionById(tEvent.TransactionInstanceId);
                //    var tKind = ProcessKind.GetTransactionById(tInstance.TransactionKindId);

                    var eventVm = new EventControlViewModel(chunkVm)
                    {
                        CreationTime = tEvent.Created.ToString(XmlParsersConfig.DateTimeFormat),
                        SelectedCompletion = ((CompletionChangedTransactionEvent) tEvent).Completion
                    };


                    //  eventVm.SelectedTransactionInstance = new TransactionViewModel(tInstance, tKind.Name);
                    eventVm.SetSelectedTransactionInstance(tInstance.Id);
                    eventVm.SetSelectedActor(tEvent.RaisedByActorId);

                    chunkVm.Events.Add(new EventControl(eventVm));
                }

                ChunkControls.Add(new ChunkControl(chunkVm));

            }
        }

        private void AddChunkCommandExecute(object obj)
        {
            ChunkControls.Add(new ChunkControl(this));
        }

        public void RemoveChunk(ChunkControl chunkControl)
        {
            ChunkControls.Remove(chunkControl);
        }

        public static ActorRole GetActorRole(int id) => ActorRoles.First(x => x.Id == id);
    }
}
