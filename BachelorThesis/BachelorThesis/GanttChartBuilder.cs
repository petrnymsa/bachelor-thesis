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

//            var l = new TransactionLinkControl(TransactionCompletion.Requested, TransactionCompletion.Requested, t1, t2, TransactionLinkOrientation.Down, TransactionLinkStyle.StateToRequest);
//            chartLayout.Children.Add(l,
//               /* xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPositionDPS(TransactionCompletion.Requested)),*/
//                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));
//            //            chartLayout.Children.Add(GetNewLink("Rq", "Rq", linkStyle: TransactionLinkStyle.StateToRequest),
//            //                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPositionDPS(TransactionCompletion.Requested)),
//            //                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));
//            l.RefreshLayout();

            TransactionLinkBuilder.New(TransactionCompletion.Requested, TransactionCompletion.Requested, t1, t2)
                .SetStyle(TransactionLinkStyle.StateToRequest)
                .Build(chartLayout, t1);

            //              var leftSpace = (float)LeftSpace(t1, TransactionCompletion.Requested);
            //              var targetCompletion = TransactionCompletion.Promised;
            //              var sourceCompletion = TransactionCompletion.Promised;
            //
            //              var sourceX = t2.GetCompletionPositionDPS(sourceCompletion);
            //              var targetX = t1.GetCompletionPositionDPS(targetCompletion) - (float)LeftSpace(t1, TransactionCompletion.Requested);
            //              var offset = sourceCompletion <= targetCompletion ? sourceX : targetX;
            //
            //           //   var offset = (float) LeftSpace(t1, TransactionCompletion.Requested) + Math.Abs(sourceX - targetX);
            //              DebugHelper.Info($"SourceX: {sourceX}, TargetX: {targetX}, WidthReqT1: {t1.WidthRequest}, WidthReqT2: {t2.WidthRequest}");
            //
            //              var link = GetNewLink("Pm", "Pm", TransactionLinkOrientation.Up, dashed: true, sourceX: sourceX, targetX: targetX);
            //              link.SourceBox = t2;
            //              link.TargetBox = t1;
            //              link.SourceCompletion = sourceCompletion;
            //              link.TargetCompletion = targetCompletion;
            //              link.IsReflected = sourceCompletion <= targetCompletion;
            //              link.BackgroundColor = Color.LightGray;

//            var targetCompletion = TransactionCompletion.Promised;
//            var sourceCompletion = TransactionCompletion.Promised;
//            var link = new TransactionLinkControl(sourceCompletion, targetCompletion, t2, t1,
//                TransactionLinkOrientation.Up, offsetCompletion: TransactionCompletion.Requested);
//            link.BackgroundColor = Color.LightGray;
//
//            chartLayout.Children.Add(link,
//                /* xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + leftSpace + offset - TransactionLinkControl.ShapeRadius),*/
//                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));
//
//            link.RefreshLayout();

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

        //private void AddLink()

        //private static TransactionLinkControl GetNewLink(string sourceText, string targetText,
        //    TransactionLinkOrientation linkOrientation = TransactionLinkOrientation.Down,
        //    TransactionLinkStyle linkStyle = TransactionLinkStyle.StateToState, bool dashed = false,
        //    float bendWidth = 0, string sourceCardinality = null, string targetCardinality = null,
        //    float sourceX = 0f, float targetX = 0f)
        //{
        //    return new TransactionLinkControl()
        //    {
        //        SourceText = sourceText,
        //        TargetText = targetText,
        //        LinkOrientation = linkOrientation,
        //        LinkStyle = linkStyle,
        //        IsDashed = dashed,
        //        BendWidth = bendWidth,
        //        SourceCardinality = sourceCardinality,
        //        TargetCardinality = targetCardinality,
        //        SourceX = sourceX,
        //        TargetX = targetX
        //    };
        //}

        //        private static double LeftSpace(TransactionBoxControl box, TransactionCompletion completion)
        //        {
        //            return box.GetCompletionPositionDPS(completion) + TransactionLinkControl.StateToRequestArrowHead + TransactionLinkControl.ShapeRadius;
        //        }

        //        private static double RightSpace(TransactionBoxControl box, TransactionCompletion completion)
        //        {
        //            return box.WidthRequest - box.GetCompletionPositionDPS(completion) - TransactionLinkControl.StateToRequestArrowHead - TransactionLinkControl.ShapeRadius;
        //        }
        //
        //        private static float GetBendWidth(TransactionBoxControl parentBox, TransactionBoxControl childBox, TransactionCompletion offsetCompletion,
        //            TransactionCompletion parentCompletion, TransactionCompletion childCompletion)
        //        {
        //            var offset = (float)LeftSpace(parentBox, offsetCompletion);
        //            var actWidthOffset = parentBox.GetCompletionPositionDPS(parentCompletion) - offset;
        //
        //            var result = childBox.GetCompletionPositionDPS(childCompletion) - actWidthOffset;
        //           // Debug.WriteLine($"[info] offset: {offset} | actOffset: {actWidthOffset} | result: {result}");
        //
        //            return result;
        //        }

        private static float GetMaximumDependendChildWidth(TransactionBoxControl parent, TransactionCompletion offset)
        {
            var leftSpace = parent.GetCompletionOffset(offset);
            return (float)(parent.WidthRequest - leftSpace);
        }
    }
}