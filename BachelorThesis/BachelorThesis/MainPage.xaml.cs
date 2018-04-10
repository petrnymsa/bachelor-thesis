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
    //public class SvgImageXamlDemoPageViewModel
    //{
    //    public Assembly SvgAssembly => typeof(App).GetTypeInfo().Assembly;
    //}

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
            //{
            //    boxT1,
            //    boxT2,
            //    boxT3,
            //    boxT4,
            //    boxT5
		    //};
		 //  chartLayout.BackgroundColor = Color.LightCyan;
		    PrepareView();
        }

        TransactionBoxControl GetNewBox(float width, int transactionId, bool isActive = false, float barHeight = 30f)
        {
            return new TransactionBoxControl()
            {
                WidthRequest = width,
                HeightRequest = barHeight,
                IsActive = isActive,
                TransactionId = transactionId
            };
        }

        TransactionLinkControl GetNewLink(string sourceText, string targetText, int linkOrientation = 0,
            int linkStyle = 0, bool dashed = false, float bendWidth = 0)
        {
            return new TransactionLinkControl()
            {
                SourceText = sourceText,
                TargetText = targetText,
                LinkOrientation = linkOrientation,
                LinkStyle = linkStyle,
                IsDashed = dashed,
                BendWidth = bendWidth

            };
        }

        double LeftSpace(TransactionBoxControl box, TransactionCompletion completion)
        {
           return box.GetCompletionPosition(completion) + 16 + 4;
        }

        double RightSpace(TransactionBoxControl box, TransactionCompletion completion)
        {
            return box.WidthRequest - box.GetCompletionPosition(completion) - 16 - 4;
        }

        float GetBendWidth(TransactionBoxControl parentBox, TransactionBoxControl childBox, TransactionCompletion offsetCompletion,
            TransactionCompletion parentCompletion, TransactionCompletion childCompletion)
        {
            var offset = (float)LeftSpace(parentBox, offsetCompletion);
            var actWidthOffset = parentBox.GetCompletionPosition(parentCompletion) - offset;

            var result = childBox.GetCompletionPosition(childCompletion) - actWidthOffset;
            Debug.WriteLine($"[info] offset: {offset} | actOffset: {actWidthOffset} | result: {result}");

            return result;
        }

        float GetMaximumDependendChildWidth(TransactionBoxControl parent, TransactionCompletion offset)
        {
            var leftSpace = LeftSpace(parent, offset);
            return (float) (parent.WidthRequest - leftSpace);
        }

        void PrepareView()
        {
            var barSpacing = 90;

            var t1 = GetNewBox(280,1);
            var t2 = GetNewBox(110, 2);
            var t3 = GetNewBox(160,3);
            var t4 = GetNewBox(200,4);
            var t5 = GetNewBox(120, 5);

            transactionBoxControls = new List<TransactionBoxControl>(){t1,t2,t3,t4,t5};

            chartLayout.Children.Add(t1,
                xConstraint: Constraint.RelativeToParent(parent => 2),
                yConstraint: Constraint.RelativeToParent(parent => parent.Height * 0.05f)
            );

            t2.WidthRequest = t1.WidthRequest - (LeftSpace(t1, TransactionCompletion.Requested) + RightSpace(t1, TransactionCompletion.Executed));

            chartLayout.Children.Add(t2,
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + LeftSpace(t1, TransactionCompletion.Requested)),
                yConstraint: Constraint.RelativeToView(t1, (parent, element) => element.Y + barSpacing)
            );

            chartLayout.Children.Add(t3,
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + element.WidthRequest),
                yConstraint: Constraint.RelativeToView(t2, (parent, element) => element.Y + 60)
            );

         //   t4.WidthRequest = t3.WidthRequest - (LeftSpace(t3, TransactionCompletion.Requested) + RightSpace(t3, TransactionCompletion.Accepted));

            chartLayout.Children.Add(t4,
                xConstraint: Constraint.RelativeToView(t3, (parent, element) => element.X + LeftSpace(t3, TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t3, (parent, element) => element.Y + barSpacing)
            );

            t5.WidthRequest = GetMaximumDependendChildWidth(t4, TransactionCompletion.Rejected);

            chartLayout.Children.Add(t5,
                xConstraint: Constraint.RelativeToView(t4, (parent, element) => element.X + LeftSpace(t4, TransactionCompletion.Rejected)),
                yConstraint: Constraint.RelativeToView(t4, (parent, element) => element.Y + barSpacing)
            );

            float lineStart = 30 - TransactionLinkControl.ShapeRadius; 

            chartLayout.Children.Add(GetNewLink("Rq", "Rq", linkStyle: 1), 
                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPosition(TransactionCompletion.Requested)),
                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Pm", "Pm", linkOrientation: 1, dashed: true, linkStyle:2 ,
               bendWidth: GetBendWidth(t1,t2, TransactionCompletion.Requested, TransactionCompletion.Promised, TransactionCompletion.Promised)),
                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPosition(TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Ac", "Ex", linkOrientation: 1, linkStyle: 2, dashed: true, 
                    bendWidth: GetBendWidth(t1,t2, TransactionCompletion.Requested, TransactionCompletion.Executed, TransactionCompletion.Accepted)),
                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPosition(TransactionCompletion.Executed)),
                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Pm", "Rq", linkStyle:1),
                xConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.X + t3.GetCompletionPosition(TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.Y + lineStart));


            chartLayout.Children.Add(GetNewLink("Pm", "Ex", linkOrientation: 1,linkStyle:2, dashed: true, 
                    bendWidth: GetBendWidth(t3,t4,TransactionCompletion.Promised, TransactionCompletion.Executed, TransactionCompletion.Promised)),
                xConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.X + t3.GetCompletionPosition(TransactionCompletion.Executed)),
                yConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.Y + lineStart));


            chartLayout.Children.Add(GetNewLink("Rj", "Rq", linkStyle: 1),
                xConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.X + t4.GetCompletionPosition(TransactionCompletion.Rejected)),
                yConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Ac", "Ac", linkOrientation: 1, linkStyle: 2, dashed: true,
                    bendWidth: GetBendWidth(t4, t5, TransactionCompletion.Rejected, TransactionCompletion.Accepted, TransactionCompletion.Accepted)),
                xConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.X + t4.GetCompletionPosition(TransactionCompletion.Accepted)),
                yConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.Y + lineStart));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var assembly = typeof(MainPage).GetTypeInfo().Assembly;


            //Stream stream = assembly.GetManifestResourceStream("BachelorThesis.SimulationFiles.case-01.xml");
            //string xml = "";
            //using (var reader = new StreamReader(stream))
            //    xml = await reader.ReadToEndAsync();

            //rentalContractSimulation = new RentalContractSimulationFromXml(xml);

            //rentalContractSimulation.Prepare();

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
            //var results = rentalContractSimulation.SimulateNextChunk();

            //if (results == null)
            //    return;


            //foreach (var evt in results)
            //{
            //    //var transaction = rentalContractSimulation.ProcessInstance.GetTransactionById(evt.TransactionInstanceId);
            //    var transactionControl = transactionBoxControls.Find(x => x.TransactionId == evt.TransactionInstanceId);
            //    if (evt.EventType != TransactionEventType.CompletionChanged) continue;

            //    var evtCompletion = (CompletionChangedTransactionEvent) evt;
            //    transactionControl.AddProgress(evtCompletion.Completion);
            //    Debug.WriteLine($"[info] Transaction {evt.TransactionInstanceId} changed state to {evtCompletion.Completion} ");
            //}

            foreach (var box in transactionBoxControls)
            {
                var start = box.Progress;
                var end = start + 0.20f;
                box.Animate("aa", x => box.Progress = (float)x, start, end, 4, 2000, Easing.Linear);
            }
        }
    }
}
