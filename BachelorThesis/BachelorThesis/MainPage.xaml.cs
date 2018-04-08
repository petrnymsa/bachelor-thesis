using System;
using System.IO;
using System.Reflection;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Simulation;
using Xamarin.Forms;

namespace BachelorThesis
{
    //public class SvgImageXamlDemoPageViewModel
    //{
    //    public Assembly SvgAssembly => typeof(App).GetTypeInfo().Assembly;
    //}

    public partial class MainPage : ContentPage
    {
        private RentalContractSimulationFromXml rentalContractSimulation;

        private float progress;

		public MainPage()
		{
			InitializeComponent();
	//	    rentalContractSimulation = new RentalContractSimulationFromXml(XmlSimulationExample.Case01());

            progress = 0f;
		//    Device.StartTimer(TimeSpan.FromMilliseconds(800), TimerTick);


		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
          

            Stream stream = assembly.GetManifestResourceStream("BachelorThesis.SimulationFiles.case-01.xml");
            string xml = "";
            using (var reader = new StreamReader(stream))
                xml =  await reader.ReadToEndAsync();

            rentalContractSimulation = new RentalContractSimulationFromXml(xml);

            rentalContractSimulation.Prepare();

        }

        private bool TimerTick()
	    {
	        var old = progress;
	        progress += 0.1f;

          //  boxT1.Animate(nameof(progress), x=> boxT1.Progress = (float)x,old,progress,4,2000,Easing.Linear);

	     //   boxT1.Progress = progress;

	        if (progress >= 1)
	            return false;

	        return true;
	    }

        private void BtnNextStep_OnClicked(object sender, EventArgs e)
        {
            var results = rentalContractSimulation.SimulateNextChunk();

            foreach (var evt in results)
            {
                var transaction =
                    rentalContractSimulation.ProcessInstance.GetTransactionById(evt.TransactionInstanceId);
                //var actor = rentalContractSimulation.FindActorById(transactionEvent.RaisedByActorId);
                ////    Console.WriteLine($"[{transactionEvent.Created}] Event '{transactionEvent.EventType}' affected transaction '{transaction.Identificator}'. Raised by '{actor.FullName}'");
                ////Console.WriteLineFormatted("[{0}] Event '{1}' affected transaction '{2}'. Raised by '{3}'", Color.Moccasin, Color.WhiteSmoke, new[]
                ////{
                ////    transactionEvent.Created.ToString(),
                ////    transactionEvent.EventType.ToString(),
                ////    transaction.Identificator,
                ////    actor.FullName
                ////});

                if (evt.EventType == TransactionEventType.CompletionChanged)
                {
                    var evtCompletion = (CompletionChangedTransactionEvent) evt;
                    
                }
            }
        }
    }
}
