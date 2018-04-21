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

            t1.AddDescendant(t2);
            t4.AddDescendant(t5);

            t2.ParentControl = t1;
            t3.ParentControl = t2;
            t4.ParentControl = t3;
            t5.ParentControl = t4;

            TransactionBoxControls = new List<TransactionBoxControl>() { t1, t2, t3, t4, t5 };

            //t1.OffsetX = 2;/
            chartLayout.Children.Add(t1,
                xConstraint: Constraint.RelativeToParent(parent => 2),
                yConstraint: Constraint.RelativeToParent(parent => parent.Height * 0.05f)
            );

          //  t2.OffsetX = (float)(t1.X +  t1.GetCompletionOffset(TransactionCompletion.Requested));
            chartLayout.Children.Add(t2,
//                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + t1.GetCompletionOffset(TransactionCompletion.Requested)),
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + t1.GetCompletionOffset(TransactionCompletion.Requested)),
                yConstraint: Constraint.RelativeToView(t1, (parent, element) => element.Y + BarSpacing)
            );

          //  t3.OffsetX = (float)(t1.X + t1.WidthRequest + 4);
            chartLayout.Children.Add(t3,
//                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + element.WidthRequest + 4),
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + t1.WidthRequest + 4),
                yConstraint: Constraint.RelativeToView(t2, (parent, element) => element.Y + 60)
            );

         //   t4.OffsetX = (float)(t3.X + t3.GetCompletionOffset(TransactionCompletion.Promised));
            chartLayout.Children.Add(t4,
//                xConstraint: Constraint.RelativeToView(t3, (parent, element) => element.X + t3.GetCompletionOffset(TransactionCompletion.Promised)),
                xConstraint: Constraint.RelativeToView(t3, (parent, element) => element.X + t3.GetCompletionOffset(TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t3, (parent, element) => element.Y + BarSpacing)
            );

            t5.WidthRequest = GetMaximumDependendChildWidth(t4, TransactionCompletion.Rejected);
         //   t5.OffsetX = (float)(t4.X + t4.GetCompletionOffset(TransactionCompletion.Rejected));

            chartLayout.Children.Add(t5,
//                xConstraint: Constraint.RelativeToView(t4, (parent, element) => element.X + t4.GetCompletionOffset(TransactionCompletion.Rejected)),
                xConstraint: Constraint.RelativeToView(t4, (parent, element) => element.X + t4.GetCompletionOffset(TransactionCompletion.Rejected)),
                yConstraint: Constraint.RelativeToView(t4, (parent, element) => element.Y + BarSpacing)
            );

            //var lineStart = BarHeight - TransactionLinkControl.ShapeRadius;

            // T1 rq - T2 eq
            t1.AddLink(NewLink (TransactionCompletion.Requested, TransactionCompletion.Requested, t1, t2)
                .SetStyle(TransactionLinkStyle.StateToRequest)
                .Build(chartLayout, t1));

            NewLink(TransactionCompletion.Promised,  TransactionCompletion.Promised, t2, t1)
                .SetOrientation(TransactionLinkOrientation.Up)
                .SetOffsetCompletion(TransactionCompletion.Requested)
                .Build(chartLayout, t1);

            NewLink(TransactionCompletion.Accepted, TransactionCompletion.Executed, t2, t1)
                .SetOrientation(TransactionLinkOrientation.Up)
                .SetOffsetCompletion(TransactionCompletion.Requested)
                .Build(chartLayout, t1);

            NewLink(TransactionCompletion.Promised, TransactionCompletion.Requested, t3, t4)
                .SetStyle(TransactionLinkStyle.StateToRequest)
                .Build(chartLayout, t3);

            NewLink(TransactionCompletion.Promised, TransactionCompletion.Executed, t4, t3)
                .SetOrientation(TransactionLinkOrientation.Up)
                .SetOffsetCompletion(TransactionCompletion.Promised)
                .Build(chartLayout, t3);

            NewLink(TransactionCompletion.Rejected, TransactionCompletion.Requested, t4, t5)
                .SetStyle(TransactionLinkStyle.StateToRequest)
                .SetSourceCardinality("0..1")
                .Build(chartLayout, t4);

            NewLink(TransactionCompletion.Accepted, TransactionCompletion.Accepted, t5, t4)
                .SetOrientation(TransactionLinkOrientation.Up)
                .SetTargetCardinality("0..1")
                .SetOffsetCompletion(TransactionCompletion.Rejected)
                .Build(chartLayout, t4);
        }


        private TransactionLinkBuilder NewLink(TransactionCompletion sourceCompletion, TransactionCompletion targetCompletion,
            TransactionBoxControl sourceControl, TransactionBoxControl targetControl) => TransactionLinkBuilder.New(sourceCompletion, targetCompletion, sourceControl, targetControl);

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