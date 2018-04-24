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
using BachelorThesis.Helpers;
using Websockets;
using Xamarin.Forms;

namespace BachelorThesis.Views
{
    public partial class ProcessVisualisationPage : ContentPage
    {
        private ProcessSimulation simulation;

        private List<TransactionBoxControl> transactionBoxControls;
        private bool timerCanRun;
        private SimulationCaseViewModel selectedCase;

        private bool simulationEnded = false;

        public bool TimerCanRun
        {
            get => timerCanRun;
            set
            {
                timerCanRun = value;

                btnPause.IsVisible = timerCanRun;
                btnPlay.IsVisible = !timerCanRun;
                btnNextStep.IsVisible = !timerCanRun;
            }
        }

        public ProcessVisualisationPage()
        {

            InitializeComponent();
            TimerCanRun = false;
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

        private async void PrepareView()
        {

            var builder = new GanttChartBuilder();
            builder.Build(chartLayout);

            transactionBoxControls = builder.TransactionBoxControls;
            await PrepareSimulation();

            TimerCanRun = false;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Send(this, "setLandscape");
        }

        private async Task PrepareSimulation()
        {
            var loader = new SimulationProvider();
            simulation = await loader.LoadAsync(selectedCase.CaseName);

            foreach (var control in transactionBoxControls)
            {
                var transaction = simulation.ProcessInstance.GetTransactionById(control.TransactionId.Value);
                control.Transaction = transaction;
            }
        }

        private bool TimerTick()
        {
            if (!TimerCanRun)
                return false;

            TimerCanRun = NextSimulationStep();

            return TimerCanRun;
        }

        private bool NextSimulationStep()
        {
            if (simulationEnded)
                Reset();

            if (!simulation.CanContinue)
            {
                simulationEnded = true;
                return false;
            }

            var results = simulation.SimulateNextChunk();
          
            foreach (var transactionEvent in results)
            {
                //   var transaction = simulation.ProcessInstance.GetTransactionById(transactionEvent.TransactionInstanceId);
                var transactionControl = transactionBoxControls.Find(x => x.TransactionId == transactionEvent.TransactionInstanceId);
                Debug.WriteLine($"[info] Transaction {transactionEvent.TransactionInstanceId} changed state to {transactionEvent.Completion} ");

                transactionControl.AddProgress(transactionEvent.Completion);

                timeLineLayout.AssociateEvent(transactionControl, transactionEvent);
            }

            return true;

        }

        private void Reset()
        {
            simulationEnded = false;
            simulation.Reset();
            timeLineLayout.Reset();

            foreach (var control in transactionBoxControls)
            {
                control.ResetProgress();
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
            NextSimulationStep();
        }

        private void BtnPause_OnClicked(object sender, EventArgs e)
        {
            TimerCanRun = false;
        }

        private void BtnPlay_OnClicked(object sender, EventArgs e)
        {
            TimerCanRun = true;
            Device.StartTimer(TimeSpan.FromSeconds(1), TimerTick);
        }
    }
}
