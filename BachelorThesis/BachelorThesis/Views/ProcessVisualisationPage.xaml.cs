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
using Xamarin.Forms;

namespace BachelorThesis.Views
{
    public partial class ProcessVisualisationPage : ContentPage
    {
        private RentalContractSimulationFromXml rentalContractSimulation;

        private List<TransactionBoxControl> transactionBoxControls;

        private bool livePreview = false;
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

            //if (livePreview)
            //{
            //    var box = transactionBoxControls.Find(x => x.TransactionId == 1);
            //    var offset = timeLineLayout.X - 100;
            //    var spaceX = box.X - offset;
            //    var move = spaceX + box.GetCompletionPosition(TransactionCompletion.Requested);

            //    //timeLineLayout.Children.Add(new BoxView()
            //    //    {
            //    //        Color = Color.Black,
            //    //        HeightRequest = 10,
            //    //        WidthRequest = 5

            //    //    }, xConstraint: Constraint.RelativeToParent((p) => move - 100)
            //    //);
            //}
           // else 
            await PrepareSimulation();
        }

        private async Task PrepareSimulation()
        {
            var assembly = typeof(ProcessVisualisationPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("BachelorThesis.SimulationFiles.case-01.xml");

            string xml = "";
            using (var reader = new StreamReader(stream))
                xml = await reader.ReadToEndAsync();
            rentalContractSimulation = new RentalContractSimulationFromXml(xml);
            rentalContractSimulation.Prepare();
        }

        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //during page close setting back to portrait
            MessagingCenter.Send(this, "unlockOrientation");
        }

        private void BtnNextStep_OnClicked(object sender, EventArgs e)
        {
            //if (livePreview)
            //{
            //    foreach (var box in transactionBoxControls)
            //    {
            //        var start = box.Progress;
            //        var end = start + 0.20f;
            //        box.Animate("aa", x => box.Progress = (float) x, start, end, 4, 1200, Easing.SinInOut);
            //    }
            //    return;
            //}

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

                var offset = timeLineLayout.X - 100; // we all love magic constants, i know
                var spaceX = transactionControl.X - offset;
                var move = spaceX + transactionControl.GetCompletionPosition(evtCompletion.Completion);

                DebugHelper.Info($"box: {transactionControl.X}, timeline: {timeLineLayout.X}, offset: {offset}, spaceX: {spaceX}, move: {move}");

                //var eventControl = new TransactionEventControl()
                //{
                //   // BackgroundColor = Color.Azure,
                    
                //    TopLabel = $"{transaction.Identificator}[{evtCompletion.Completion.AsAbbreviation()}]",
                //  //  BottomLabel = $"{evt.Created.Day}.{evt.Created.Month} {evt.Created.Hour}:{evt}",
                //    BottomLabel = evt.Created.ToString("dd.MM HH:mm:ss"),
                //    Completion = evtCompletion.Completion

                //};

                //transactionControl.AssociateEvent(eventControl);

               var timeLineEvent = timeLineLayout.AddEvent(move, transaction.Identificator, evtCompletion, transactionControl.HighlightColor);
                transactionControl.AssociateEvent(timeLineEvent, evtCompletion.Completion);

                //timeLineLayout.Children.Add(eventControl,
                //    xConstraint: Constraint.RelativeToParent(p => move - eventControl.Width / 2),
                //    yConstraint: Constraint.RelativeToParent(p => p.Height * 0.5f - eventControl.HeightRequest / 2f));
                //timeLineLayout.Children.Add(new BoxView()
                //    {
                //        Color = Color.Azure,
                //        WidthRequest = 3,
                //        HeightRequest = 40

                //    }, xConstraint: Constraint.RelativeToParent((p) => move )
                //);
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
            //Debug.WriteLine($"[info] Scroll x: {e.ScrollX} y: {e.ScrollY}");
            //Debug.WriteLine($"[info] timeLine y: {timeLineLayout.Y}");
            //Debug.WriteLine($"[info] main y: {mainLayout.Y}");

            // timeLineLayout.TranslateTo(mainLayout.Y + e.ScrollY, e.ScrollY, 0);

            RelativeLayout.SetYConstraint(timeLineLayout, Constraint.RelativeToParent((parent) => parent.Y + e.ScrollY));
           // RelativeLayout.SetYConstraint(timeLineLayout, Constraint.RelativeToParent((parent) => parent.Y + e.ScrollY));
        }

        private void BtnTest_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TestPage());
        }
    }
}
