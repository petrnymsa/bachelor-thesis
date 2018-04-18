using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Business.Simulation;
using BachelorThesis.Controls;
using Websockets;
using Xamarin.Forms;

namespace BachelorThesis.Views
{
    public partial class ProcessVisualisationPage : ContentPage
    {
        private RentalContractSimulationFromXml rentalContractSimulation;

        private List<TransactionBoxControl> transactionBoxControls;

        private bool livePreview = true;
        private List<TimeLineItem> items = new List<TimeLineItem>();

        public ProcessVisualisationPage()
        {

            InitializeComponent();

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
            MessagingCenter.Send(this, "setLandscape");

            await PrepareSimulation();
        }

        private async Task PrepareSimulation()
        {
            if (livePreview)
            {
                var xml = await SimulationCases.LoadXmlAsync(SimulationCases.Case01);
                rentalContractSimulation = new RentalContractSimulationFromXml(xml);
                rentalContractSimulation.Prepare();
            }
            else
            {
                var assembly = typeof(SimulationCases).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(SimulationCases.Case01);

                string xml = "";
                using (var reader = new StreamReader(stream))
                    xml = await reader.ReadToEndAsync();
                rentalContractSimulation = new RentalContractSimulationFromXml(xml);
                rentalContractSimulation.Prepare();
            }
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //during page close setting back to portrait
            MessagingCenter.Send(this, "unlockOrientation");
        }

        private async void BtnNextStep_OnClicked(object sender, EventArgs e)
        {

            //            foreach (var boxControl in transactionBoxControls)
            //            {
            //                var start = boxControl.Progress;
            //                var end = start + 0.2f;
            //                boxControl.Animate("a", x => boxControl.Progress = (float)x, start, end, 4, 1000, Easing.Linear);
            //            }

            var results = rentalContractSimulation.SimulateNextChunk();

            if (results == null)
                return;


            foreach (var evt in results)
            {
                var transaction = rentalContractSimulation.ProcessInstance.GetTransactionById(evt.TransactionInstanceId);
                var transactionControl = transactionBoxControls.Find(x => x.TransactionId == evt.TransactionInstanceId);
                if (evt.EventType != TransactionEventType.CompletionChanged) continue;

                var evtCompletion = (CompletionChangedTransactionEvent)evt;
                transactionControl.AddProgress(evtCompletion.Completion);
                Debug.WriteLine($"[info] Transaction {evt.TransactionInstanceId} changed state to {evtCompletion.Completion} ");

                var offset = timeLineLayout.X; //- 100; // we all love magic constants, i know
                var spaceX = transactionControl.X - offset;
                var move = spaceX + transactionControl.GetCompletionPositionDPS(evtCompletion.Completion);

                DebugHelper.Info($"box: {transactionControl.X}, timeline: {timeLineLayout.X}, offset: {offset}, spaceX: {spaceX}, move: {move}");


                var timeLineEvent = timeLineLayout.AddEvent(move, transaction.Identificator, evtCompletion, transactionControl.HighlightColor);
                transactionControl.AssociateEvent(timeLineEvent, evtCompletion.Completion);
            }

        }

        private void BtnClear_OnClicked(object sender, EventArgs e)
        {
            //            foreach (var boxControl in transactionBoxControls)
            //            {
            //                boxControl.Progress = 0;
            //            }
            //
            //            rentalContractSimulation.Reset();
        }

        private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
        {
            RelativeLayout.SetYConstraint(timeLineLayout, Constraint.RelativeToParent((parent) => parent.Y + e.ScrollY));
        }
    }
}
