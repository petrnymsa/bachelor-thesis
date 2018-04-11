using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Simulation;
using BachelorThesis.Controls;
using Xamarin.Forms;

namespace BachelorThesis
{
    public partial class MainPage : ContentPage
    {
        private RentalContractSimulationFromXml rentalContractSimulation;

        private float progress;

        private List<TransactionBoxControl> transactionBoxControls; 

		public MainPage()
		{
		   
            InitializeComponent();
            progress = 0f;

		    transactionBoxControls = new List<TransactionBoxControl>();

		    PrepareView();
        }

        private void PrepareView()
        {

            var builder = new GanttChartBuilder();
            builder.Build(chartLayout);

            transactionBoxControls = builder.TransactionBoxControls;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var assembly = typeof(MainPage).GetTypeInfo().Assembly;


//            Stream stream = assembly.GetManifestResourceStream("BachelorThesis.SimulationFiles.case-01.xml");
//
//            string xml = "";
//            using (var reader = new StreamReader(stream))
//                xml = await reader.ReadToEndAsync();
//
//            rentalContractSimulation = new RentalContractSimulationFromXml(xml);
//
//            rentalContractSimulation.Prepare();

        }

        private void BtnNextStep_OnClicked(object sender, EventArgs e)
        {
//            var results = rentalContractSimulation.SimulateNextChunk();
//
//            if (results == null)
//                return;
//
//
//            foreach (var evt in results)
//            {
//                //var transaction = rentalContractSimulation.ProcessInstance.GetTransactionById(evt.TransactionInstanceId);
//                var transactionControl = transactionBoxControls.Find(x => x.TransactionId == evt.TransactionInstanceId);
//                if (evt.EventType != TransactionEventType.CompletionChanged) continue;
//
//                var evtCompletion = (CompletionChangedTransactionEvent)evt;
//                transactionControl.AddProgress(evtCompletion.Completion);
//                Debug.WriteLine($"[info] Transaction {evt.TransactionInstanceId} changed state to {evtCompletion.Completion} ");
//            }

            foreach (var box in transactionBoxControls)
            {
                var start = box.Progress;
                var end = start + 0.20f;
                box.Animate("aa", x => box.Progress = (float)x, start, end, 4, 1200, Easing.SinInOut);
            }
        }

        private void BtnClear_OnClicked(object sender, EventArgs e)
        {
            foreach (var boxControl in transactionBoxControls)
            {
                boxControl.Progress = 0;
            }

            rentalContractSimulation.Reset();
        }

        private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
        {
            Debug.WriteLine($"[info] Scroll x: {e.ScrollX} y: {e.ScrollY}");
            Debug.WriteLine($"[info] timeLine y: {timeLineLayout.Y}");
            Debug.WriteLine($"[info] main y: {mainLayout.Y}");

            // timeLineLayout.TranslateTo(mainLayout.Y + e.ScrollY, e.ScrollY, 0);

            RelativeLayout.SetYConstraint(timeLineLayout, Constraint.RelativeToParent((parent) => parent.Y + e.ScrollY));
           // RelativeLayout.SetYConstraint(timeLineLayout, Constraint.RelativeToParent((parent) => parent.Y + e.ScrollY));
        }
    }
}
