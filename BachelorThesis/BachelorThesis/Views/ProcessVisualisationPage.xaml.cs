using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private ProcessSimulation simulation;

        private List<TransactionBoxControl> transactionBoxControls;

        private bool livePreview = true;
     //   private List<HourMinuteAnchor> items = new List<HourMinuteAnchor>();

        public ProcessVisualisationPage()
        {

            InitializeComponent();
            transactionBoxControls = new List<TransactionBoxControl>();
            selectedCase = new SimulationCaseViewModel("Case -01", SimulationCases.Case01, "...");
            this.Title = selectedCase.Name;

            PrepareView();
        }

        public ProcessVisualisationPage(SimulationCaseViewModel selectedCase)
        {
            InitializeComponent();
            this.selectedCase = selectedCase;
            this.Title = selectedCase.Name;
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
            var loader = new SimulationProvider();
            simulation = await loader.LoadAsync(SimulationCases.Case01);

            foreach (var control in transactionBoxControls)
            {
                var transaction = simulation.ProcessInstance.GetTransactionById(control.TransactionId.Value);
                control.Transaction = transaction;
            }
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //during page close setting back to portrait
            MessagingCenter.Send(this, "unlockOrientation");
        }

        private void BtnNextStep_OnClicked(object sender, EventArgs e)
        {
            var results = simulation.SimulateNextChunk();
            if (results == null)
            {
                DisplayAlert("Simulation ended", "No more simulation steps", "Ok");
                return;
            }

            foreach (var transactionEvent in results)
            {
                var transaction = simulation.ProcessInstance.GetTransactionById(transactionEvent.TransactionInstanceId);
                var transactionControl = transactionBoxControls.Find(x => x.TransactionId == transactionEvent.TransactionInstanceId);
                Debug.WriteLine($"[info] Transaction {transactionEvent.TransactionInstanceId} changed state to {transactionEvent.Completion} ");

                transactionControl.AddProgress(transactionEvent.Completion);

                timeLineLayout.AssociateEvent(transactionControl, transactionEvent);
            }

        }

        private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
        {
            RelativeLayout.SetYConstraint(timeLineLayout, Constraint.RelativeToParent((parent) => parent.Y + e.ScrollY));
        }
    }
}
