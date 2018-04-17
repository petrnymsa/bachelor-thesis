using System;
using System.Collections.Generic;
using System.Diagnostics;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Controls;
using Xamarin.Forms;

namespace BachelorThesis
{

    public class TransactionLinkBuilder
    {
        private TransactionLinkControl link;

        private TransactionLinkBuilder(TransactionCompletion sourceCompletion, TransactionCompletion targetCompletion,
            TransactionBoxControl sourceControl, TransactionBoxControl targetControl)
        {
            link = new TransactionLinkControl(sourceCompletion, targetCompletion, sourceControl, targetControl);
        }

        public static TransactionLinkBuilder New(TransactionCompletion sourceCompletion, TransactionCompletion targetCompletion,
            TransactionBoxControl sourceControl, TransactionBoxControl targetControl) => new TransactionLinkBuilder(sourceCompletion, targetCompletion, sourceControl, targetControl);

        public TransactionLinkBuilder SetOrientation(TransactionLinkOrientation orientation)
        {
            link.LinkOrientation = orientation;
            if (orientation == TransactionLinkOrientation.Up)
                link.IsDashed = true;
            return this;
        }

        public TransactionLinkBuilder SetStyle(TransactionLinkStyle style)
        {
            link.LinkStyle = style;
            return this;
        }

        public TransactionLinkBuilder SetOffsetCompletion(TransactionCompletion completion)
        {
            link.OffsetCompletion = completion;
            return this;
        }

        public TransactionLinkBuilder SetSourceCardinality(string cardinality)
        {
            link.SourceCardinality = cardinality;
            return this;
        }
        public TransactionLinkBuilder SetTargetCardinality(string cardinality)
        {
            link.TargetCardinality = cardinality;
            return this;
        }

        public TransactionLinkControl Build(RelativeLayout layout, TransactionBoxControl parent, float lineStart = 26)
        {
            layout.Children.Add(link,
                yConstraint: Constraint.RelativeToView(parent, (p, sibling) => sibling.Y + lineStart));
            link.RefreshLayout();
            return link;
        }

        public void Reset() => link = null;
    }

    public class GanttChartBuilder
    {
        public List<TransactionBoxControl> TransactionBoxControls { get; private set; }

        public float BarSpacing { get; set; } = 90;
        public float BarHeight { get; set; } = 30;

        public GanttChartBuilder()
        {

        }

        public void Build(RelativeLayout chartLayout)
        {
            //var t1 = GetNewBox(350, 1, (Color)App.Current.Resources["TransactionColor1"]);
            //var t2 = GetNewBox(200, 2, (Color)App.Current.Resources["TransactionColor2"]);
            //var t3 = GetNewBox(200, 3, (Color)App.Current.Resources["TransactionColor3"]);
            //var t4 = GetNewBox(260, 4, (Color)App.Current.Resources["TransactionColor4"]);
            //var t5 = GetNewBox(160, 5, (Color)App.Current.Resources["TransactionColor5"]);

            var t1 = GetNewBox(350, 1, Color.Black);
            var t2 = GetNewBox(200, 2, Color.Black);
            var t3 = GetNewBox(200, 3, Color.Black);
            var t4 = GetNewBox(260, 4, Color.Black);
            var t5 = GetNewBox(160, 5, Color.Black);

            TransactionBoxControls = new List<TransactionBoxControl>() { t1, t2, t3, t4, t5 };

            chartLayout.Children.Add(t1,
                xConstraint: Constraint.RelativeToParent(parent => 2),
                yConstraint: Constraint.RelativeToParent(parent => parent.Height * 0.05f)
            );

            chartLayout.Children.Add(t2,
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + t1.GetCompletionOffset(TransactionCompletion.Requested)),
                yConstraint: Constraint.RelativeToView(t1, (parent, element) => element.Y + BarSpacing)
            );

            chartLayout.Children.Add(t3,
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + element.WidthRequest + 4),
                yConstraint: Constraint.RelativeToView(t2, (parent, element) => element.Y + 60)
            );

            chartLayout.Children.Add(t4,
                xConstraint: Constraint.RelativeToView(t3, (parent, element) => element.X + t3.GetCompletionOffset(TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t3, (parent, element) => element.Y + BarSpacing)
            );

            t5.WidthRequest = GetMaximumDependendChildWidth(t4, TransactionCompletion.Rejected);

            chartLayout.Children.Add(t5,
                xConstraint: Constraint.RelativeToView(t4, (parent, element) => element.X + t4.GetCompletionOffset(TransactionCompletion.Rejected)),
                yConstraint: Constraint.RelativeToView(t4, (parent, element) => element.Y + BarSpacing)
            );

            var lineStart = BarHeight - TransactionLinkControl.ShapeRadius;

            TransactionLinkBuilder.New(TransactionCompletion.Requested, TransactionCompletion.Requested, t1, t2)
                .SetStyle(TransactionLinkStyle.StateToRequest)
                .Build(chartLayout, t1);

            TransactionLinkBuilder.New(TransactionCompletion.Promised,  TransactionCompletion.Accepted, t2, t1)
                .SetOrientation(TransactionLinkOrientation.Up)
                .SetOffsetCompletion(TransactionCompletion.Requested)
                .Build(chartLayout, t1);



            //            chartLayout.Children.Add(GetNewLink("Ac", "Ex", TransactionLinkOrientation.Up, dashed: true,
            //                    bendWidth: GetBendWidth(t1, t2, TransactionCompletion.Requested, TransactionCompletion.Executed, TransactionCompletion.Accepted)),
            //                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPositionDPS(TransactionCompletion.Executed)),
            //                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));
            //
            //            chartLayout.Children.Add(GetNewLink("Pm", "Rq", linkStyle: TransactionLinkStyle.StateToRequest),
            //                xConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.X + t3.GetCompletionPositionDPS(TransactionCompletion.Promised)),
            //                yConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.Y + lineStart));
            //
            //
            //            chartLayout.Children.Add(GetNewLink("Pm", "Ex", TransactionLinkOrientation.Up, dashed: true,
            //                    bendWidth: GetBendWidth(t3, t4, TransactionCompletion.Promised, TransactionCompletion.Executed, TransactionCompletion.Promised)),
            //                xConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.X + t3.GetCompletionPositionDPS(TransactionCompletion.Executed)),
            //                yConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.Y + lineStart));
            //
            //
            //            chartLayout.Children.Add(GetNewLink("Rj", "Rq", linkStyle: TransactionLinkStyle.StateToRequest, sourceCardinality: "0..1"),
            //                xConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.X + t4.GetCompletionPositionDPS(TransactionCompletion.Rejected)),
            //                yConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.Y + lineStart));
            //
            //            chartLayout.Children.Add(GetNewLink("Ac", "Ac", TransactionLinkOrientation.Up, dashed: true, sourceCardinality: "0..1", targetCardinality: "0..1",
            //                    bendWidth: GetBendWidth(t4, t5, TransactionCompletion.Rejected, TransactionCompletion.Accepted, TransactionCompletion.Accepted)),
            //                xConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.X + t4.GetCompletionPositionDPS(TransactionCompletion.Accepted)),
            //                yConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.Y + lineStart));
        }


        private TransactionBoxControl GetNewBox(float width, int transactionId, Color color, bool isActive = false)
        {
            return new TransactionBoxControl
            {
                WidthRequest = width,
                HeightRequest = BarHeight,
                IsActive = isActive,
                TransactionId = transactionId,
                HighlightColor = color
            };
        }
        
        private static float GetMaximumDependendChildWidth(TransactionBoxControl parent, TransactionCompletion offset)
        {
            var leftSpace = parent.GetCompletionOffset(offset);
            return (float)(parent.WidthRequest - leftSpace);
        }
    }
}