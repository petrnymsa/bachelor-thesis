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
        public double AnchorSpacing { get; set; } = 20;
        public double TimeLineOffset { get; set; }

    //    private List<TimeLineItem> items;

        private int? lastDay = null;
        private int? lastMonth = null;

        private List<TimeLineAnchor> anchors;
        private double lastOffset;

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
            var month = transactionEvent.Created.Month;
            var day = transactionEvent.Created.Day;
            var hour = transactionEvent.Created.Hour;
            var minute = transactionEvent.Created.Minute;
//
//            var overlap = IsOverllap(hour, minute);
//
//            if (overlap != null)
//                return overlap.AddEvent(boxControl.Transaction.Identificator, transactionEvent.Completion, boxControl.HighlightColor);


            var item = new TimeLineItem(hour, minute);
            var timeLineEvent = item.AddEvent(boxControl.Transaction.Identificator, transactionEvent.Completion, boxControl.HighlightColor);

            item.Offset = lastOffset;
         //   items.Add(item);
            anchors.Add(item);

            if (lastDay == null || day != lastDay || month != lastMonth)
            {
                lastDay = day;
                lastMonth = month;

                AddSeparator(lastOffset, day, month, item);
                lastOffset += 2;
            }

            layout.Children.Add(item, xConstraint: Constraint.RelativeToParent(p => TimeLineOffset + lastOffset - item.WidthRequest / 2f));
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
            layout.Children.Add(separator,
                xConstraint: Constraint.RelativeToParent(p => TimeLineOffset +  separatorOffset - item.WidthRequest / 2f - separator.WidthRequest / 2f));
        }
    }
}