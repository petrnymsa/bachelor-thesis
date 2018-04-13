using System.Collections.Generic;
using System.Diagnostics;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Controls;
using Xamarin.Forms;

namespace BachelorThesis
{
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

            var t1 = GetNewBox(350, 1, (Color)App.Current.Resources["TransactionColor1"]);
            var t2 = GetNewBox(200, 2, (Color)App.Current.Resources["TransactionColor2"]);
            var t3 = GetNewBox(200, 3, (Color)App.Current.Resources["TransactionColor3"]);
            var t4 = GetNewBox(260, 4, (Color)App.Current.Resources["TransactionColor4"]);
            var t5 = GetNewBox(160, 5, (Color)App.Current.Resources["TransactionColor5"]);

            TransactionBoxControls = new List<TransactionBoxControl>() { t1, t2, t3, t4, t5 };

            chartLayout.Children.Add(t1,
                xConstraint: Constraint.RelativeToParent(parent => 2),
                yConstraint: Constraint.RelativeToParent(parent => parent.Height * 0.05f)
            );
            
            chartLayout.Children.Add(t2,
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + LeftSpace(t1, TransactionCompletion.Requested)),
                yConstraint: Constraint.RelativeToView(t1, (parent, element) => element.Y + BarSpacing)
            );

            chartLayout.Children.Add(t3,
                xConstraint: Constraint.RelativeToView(t1, (parent, element) => element.X + element.WidthRequest + 4),
                yConstraint: Constraint.RelativeToView(t2, (parent, element) => element.Y + 60)
            );

            chartLayout.Children.Add(t4,
                xConstraint: Constraint.RelativeToView(t3, (parent, element) => element.X + LeftSpace(t3, TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t3, (parent, element) => element.Y + BarSpacing)
            );

            t5.WidthRequest = GetMaximumDependendChildWidth(t4, TransactionCompletion.Rejected);

            chartLayout.Children.Add(t5,
                xConstraint: Constraint.RelativeToView(t4, (parent, element) => element.X + LeftSpace(t4, TransactionCompletion.Rejected)),
                yConstraint: Constraint.RelativeToView(t4, (parent, element) => element.Y + BarSpacing)
            );

            var lineStart = BarHeight - TransactionLinkControl.ShapeRadius;

            chartLayout.Children.Add(GetNewLink("Rq", "Rq", linkStyle: TransactionLinkStyle.StateToRequest),
                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPosition(TransactionCompletion.Requested)),
                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Pm", "Pm", TransactionLinkOrientation.Up, dashed: true,
                    bendWidth: GetBendWidth(t1, t2, TransactionCompletion.Requested, TransactionCompletion.Promised, TransactionCompletion.Promised)),
                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPosition(TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Ac", "Ex", TransactionLinkOrientation.Up, dashed: true,
                    bendWidth: GetBendWidth(t1, t2, TransactionCompletion.Requested, TransactionCompletion.Executed, TransactionCompletion.Accepted)),
                xConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.X + t1.GetCompletionPosition(TransactionCompletion.Executed)),
                yConstraint: Constraint.RelativeToView(t1, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Pm", "Rq", linkStyle: TransactionLinkStyle.StateToRequest),
                xConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.X + t3.GetCompletionPosition(TransactionCompletion.Promised)),
                yConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.Y + lineStart));


            chartLayout.Children.Add(GetNewLink("Pm", "Ex", TransactionLinkOrientation.Up, dashed: true,
                    bendWidth: GetBendWidth(t3, t4, TransactionCompletion.Promised, TransactionCompletion.Executed, TransactionCompletion.Promised)),
                xConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.X + t3.GetCompletionPosition(TransactionCompletion.Executed)),
                yConstraint: Constraint.RelativeToView(t3, (parent, sibling) => sibling.Y + lineStart));


            chartLayout.Children.Add(GetNewLink("Rj", "Rq", linkStyle: TransactionLinkStyle.StateToRequest, sourceCardinality: "0..1"),
                xConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.X + t4.GetCompletionPosition(TransactionCompletion.Rejected)),
                yConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.Y + lineStart));

            chartLayout.Children.Add(GetNewLink("Ac", "Ac", TransactionLinkOrientation.Up, dashed: true, sourceCardinality: "0..1", targetCardinality: "0..1",
                    bendWidth: GetBendWidth(t4, t5, TransactionCompletion.Rejected, TransactionCompletion.Accepted, TransactionCompletion.Accepted)),
                xConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.X + t4.GetCompletionPosition(TransactionCompletion.Accepted)),
                yConstraint: Constraint.RelativeToView(t4, (parent, sibling) => sibling.Y + lineStart));
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

        private static TransactionLinkControl GetNewLink(string sourceText, string targetText, TransactionLinkOrientation linkOrientation = TransactionLinkOrientation.Down,
            TransactionLinkStyle linkStyle = TransactionLinkStyle.StateToState, bool dashed = false, float bendWidth = 0, string sourceCardinality = null, string targetCardinality = null)
        {
            return new TransactionLinkControl()
            {
                SourceText = sourceText,
                TargetText = targetText,
                LinkOrientation = (int)linkOrientation,
                LinkStyle = (int)linkStyle,
                IsDashed = dashed,
                BendWidth = bendWidth,
                SourceCardinality = sourceCardinality,
                TargetCardinality = targetCardinality

            };
        }

        private static double LeftSpace(TransactionBoxControl box, TransactionCompletion completion)
        {
            return box.GetCompletionPosition(completion) + TransactionLinkControl.StateToRequestArrowHead + TransactionLinkControl.ShapeRadius;
        }

        private static double RightSpace(TransactionBoxControl box, TransactionCompletion completion)
        {
            return box.WidthRequest - box.GetCompletionPosition(completion) - TransactionLinkControl.StateToRequestArrowHead - TransactionLinkControl.ShapeRadius;
        }

        private static float GetBendWidth(TransactionBoxControl parentBox, TransactionBoxControl childBox, TransactionCompletion offsetCompletion,
            TransactionCompletion parentCompletion, TransactionCompletion childCompletion)
        {
            var offset = (float)LeftSpace(parentBox, offsetCompletion);
            var actWidthOffset = parentBox.GetCompletionPosition(parentCompletion) - offset;

            var result = childBox.GetCompletionPosition(childCompletion) - actWidthOffset;
           // Debug.WriteLine($"[info] offset: {offset} | actOffset: {actWidthOffset} | result: {result}");

            return result;
        }

        private static float GetMaximumDependendChildWidth(TransactionBoxControl parent, TransactionCompletion offset)
        {
            var leftSpace = LeftSpace(parent, offset);
            return (float)(parent.WidthRequest - leftSpace);
        }
    }
}