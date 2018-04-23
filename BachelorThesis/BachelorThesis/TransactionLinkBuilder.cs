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
}