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
    public static class FooClass
    {
        public const string FOO = "asasa";
    }
    public partial class ProcessVisualisationPageLive : ContentPage
    {
        private RentalContractSimulationFromXml rentalContractSimulation;

        private List<TransactionBoxControl> transactionBoxControls;

     

        private bool livePreview = true;
        private List<TimeLineItem> items = new List<TimeLineItem>();

        public ProcessVisualisationPageLive()
        {

            InitializeComponent();

            var slider = new Slider(100, 500, 350)
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            slider.ValueChanged += Slider_OnValueChanged;
            controlsLayout.Children.Add(slider);

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
           // MessagingCenter.Send(this, "setLandscape");

          //  await PrepareSimulation();
        }

        private async Task PrepareSimulation()
        {
            if (livePreview)
            {
                var liveCase = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Simulation Name=""Case01"">
  <Actors>
    <Actor Id=""1"" ActorRoleId=""1"" FirstName=""George"" LastName=""Lucas"" />
    <Actor Id=""2"" ActorRoleId=""2"" FirstName=""George"" LastName=""Lucas"" />
    <Actor Id=""3"" ActorRoleId=""3"" FirstName=""Bob"" LastName=""Freeman"" />
    <Actor Id=""4"" ActorRoleId=""4"" FirstName=""Alice"" LastName=""Freeman"" />
  </Actors>
  <ProcessInstance Id=""1"" KindId=""1"" StartTime=""01-02-2018 15:34:23"" ExpectedEndTime=""01-02-2018 15:34:23"">
    <TransactionInstance Id=""1"" KindId=""1"" Identificator=""T1"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""0"">
      <TransactionInstance Id=""2"" KindId=""2"" Identificator=""T2"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""1"" />
    </TransactionInstance>
    <TransactionInstance Id=""3"" KindId=""3"" Identificator=""T3"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""0"">
      <TransactionInstance Id=""4"" KindId=""4"" Identificator=""T4"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""3"" />
      <TransactionInstance Id=""5"" KindId=""5"" Identificator=""T5"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""3"" />
    </TransactionInstance>
  </ProcessInstance>
  <Chunks>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""1"" Created=""01-02-2018 09:00:00"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""4"" Created=""01-02-2018 09:01:00"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""1"" Created=""01-02-2018 09:01:10"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""4"" Created=""01-02-2018 09:01:30"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""1"" Created=""01-02-2018 09:01:40"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""1"" Created=""01-02-2018 09:02:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""4"" Created=""01-02-2018 09:02:15"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""4"" Created=""01-02-2018 09:02:30"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""4"" Created=""01-02-2018 09:20:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""1"" Created=""01-02-2018 09:30:05"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""2"" Created=""03-02-2018 08:30:00"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""3"" Created=""03-02-2018 08:31:00"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""3"" Created=""03-02-2018 08:31:20"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""2"" Created=""03-02-2018 08:31:55"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""3"" Created=""03-02-2018 08:32:00"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""3"" Created=""03-02-2018 08:36:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""2"" Created=""03-02-2018 08:40:50"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""2"" Created=""01-04-2018 16:30:00"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""2"" Created=""01-04-2018 16:31:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4"" RaisedById=""3"" Created=""01-04-2018 16:45:00"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
  </Chunks>
</Simulation>";


                rentalContractSimulation = new RentalContractSimulationFromXml(liveCase);
                rentalContractSimulation.Prepare();
            }
            else
            {
                //                var assembly = typeof(SimulationCases).GetTypeInfo().Assembly;
                //                Stream stream = assembly.GetManifestResourceStream(SimulationCases.Case01);
                //
                //                string xml = "";
                //                using (var reader = new StreamReader(stream))
                //                    xml = await reader.ReadToEndAsync();
                //                rentalContractSimulation = new RentalContractSimulationFromXml(xml);
                //                rentalContractSimulation.Prepare();
            }


        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //during page close setting back to portrait
          //  MessagingCenter.Send(this, "unlockOrientation");
        }

        private async void BtnNextStep_OnClicked(object sender, EventArgs e)
        {

            //foreach (var boxControl in transactionBoxControls)
            //{
            //    var start = boxControl.Progress;
            //    var end = start + 0.2f;
            //    boxControl.Animate("a", x => boxControl.Progress = (float)x, start, end, 4, 1000, Easing.Linear);
            //}

//            var results = rentalContractSimulation.SimulateNextChunk();
//
//            if (results == null)
//                return;
//
//
//            foreach (var evt in results)
//            {
//                var transaction = rentalContractSimulation.ProcessInstance.GetTransactionById(evt.TransactionInstanceId);
//                var transactionControl = transactionBoxControls.Find(x => x.TransactionId == evt.TransactionInstanceId);
//                if (evt.EventType != TransactionEventType.CompletionChanged) continue;
//
//                var evtCompletion = (CompletionChangedTransactionEvent)evt;
//                transactionControl.AddProgress(evtCompletion.Completion);
//                Debug.WriteLine($"[info] Transaction {evt.TransactionInstanceId} changed state to {evtCompletion.Completion} ");
//
//                var offset = timeLineLayout.X; //- 100; // we all love magic constants, i know
//                var spaceX = transactionControl.X - offset;
//                var move = spaceX + transactionControl.GetCompletionPositionDPS(evtCompletion.Completion);
//
//                DebugHelper.Info($"box: {transactionControl.X}, timeline: {timeLineLayout.X}, offset: {offset}, spaceX: {spaceX}, move: {move}");
//
//
//                var timeLineEvent = timeLineLayout.AddEvent(move, transaction.Identificator, evtCompletion, transactionControl.HighlightColor);
//                transactionControl.AssociateEvent(timeLineEvent, evtCompletion.Completion);
//            }

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

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
           // transactionBoxControls.First().WidthRequest = e.NewValue;
            transactionBoxControls[2].WidthRequest = e.NewValue;
        }
    }
}
