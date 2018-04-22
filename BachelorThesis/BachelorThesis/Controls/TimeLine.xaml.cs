using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeLine : ContentView
    {
        public const float AnchorSpacing = 60;
        public float TimeLineOffset { get; set; }

    //    private List<TimeLineItem> items;

        private int? lastDay = null;
        private int? lastMonth = null;

        private List<TimeLineAnchor> anchors;
        private float lastOffset = 0;
        private bool initialized = false;
        public TimeLine()
        {
            InitializeComponent();
         //   items = new List<TimeLineItem>();
            anchors = new List<TimeLineAnchor>();
        }

        //protected TimeLineItem IsOverllap(int hour, int minute)
        //{
        //    return anchors.FirstOrDefault(x => x.Hour == hour && x.Minute == minute);
        //}

        public TimeLineEvent AssociateEvent(TransactionBoxControl boxControl, CompletionChangedTransactionEvent transactionEvent)
        {
            if (!initialized)
            {
                initialized = true;
                lastOffset = TimeLineOffset;
            }
            var month = transactionEvent.Created.Month;
            var day = transactionEvent.Created.Day;
            var hour = transactionEvent.Created.Hour;
            var minute = transactionEvent.Created.Minute;
            //
            //            var overlap = IsOverllap(hour, minute);
            //
            //            if (overlap != null)
            //                return overlap.AddEvent(boxControl.Transaction.Identificator, transactionEvent.Completion, boxControl.HighlightColor);
            if (lastDay == null || day != lastDay || month != lastMonth)
            {
                lastDay = day;
                lastMonth = month;

                AddSeparator(lastOffset, day, month,null);
                lastOffset += 2;
            }

            var item = new TimeLineItem(TimeLineOffset, lastOffset, hour, minute)
            {
                Completion = transactionEvent.Completion
            };

            var timeLineEvent = item.AddEvent(boxControl.Transaction.Identificator, transactionEvent.Completion, boxControl.HighlightColor);

          //  item.TotalOffsetX = lastOffset;
         //   items.Add(item);
            anchors.Add(item);

//            layout.Children.Add(item, xConstraint: Constraint.RelativeToParent(p => item.TotalOffsetX - item.WidthRequest / 2f));
            layout.Children.Add(item, xConstraint: Constraint.RelativeToParent(p => item.TotalOffsetX));
        //    boxControl.AssociateEvent(item);
            lastOffset += AnchorSpacing;
            return timeLineEvent;
        }

        private void AddSeparator(double separatorOffset, int day, int month, TimeLineItem item)
        {
            var separator = new TimeLineSeparator()
            {
                Day = day,
                Month = month
            };
//            layout.Children.Add(separator,xConstraint: Constraint.RelativeToParent(p => separatorOffset - item.WidthRequest / 2f - separator.WidthRequest / 2f));
            layout.Children.Add(separator,xConstraint: Constraint.RelativeToParent(p => separatorOffset));
        }

        public void PrepareTimeLine(DateTime start, DateTime end)
        {
            var day = start.Day;
            var month = start.Month;

            var hour = start.Hour;
            var minute = start.Minute;

            lastOffset = TimeLineOffset;

            AddSeparator(lastOffset, day, month, null);
            lastOffset += 2;

            while (hour < 24)
            {
                while (minute != 60)
                {
                    var anchor = new TimeLineItem(TimeLineOffset, lastOffset, hour, minute);
                    anchors.Add(anchor);
                    lastOffset += AnchorSpacing;
                    minute++;
                }
                hour++;
            }

            foreach (var anchor in anchors)
            {
                layout.Children.Add(anchor, xConstraint: Constraint.RelativeToParent(p => anchor.TotalOffsetX));
            }
        }
    }
}