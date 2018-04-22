using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeLine : ContentView
    {
        public const float AnchorSpacing = 60;
        // public float TimeLineOffset { get; set; }

        //    private List<HourMinuteAnchor> items;

        private int? lastDay = null;
        private int? lastMonth = null;

        private List<TimeLineAnchor> anchors;
        private float lastOffset = 0;
        private bool initialized = false;
        public TimeLine()
        {
            InitializeComponent();
            //   items = new List<HourMinuteAnchor>();
            anchors = new List<TimeLineAnchor>();
        }

        //protected HourMinuteAnchor IsOverllap(int hour, int minute)
        //{
        //    return anchors.FirstOrDefault(x => x.Hour == hour && x.Minute == minute);
        //}

        public void AssociateEvent(TransactionBoxControl boxControl, CompletionChangedTransactionEvent transactionEvent)
        {
            if (!initialized)
            {
                initialized = true;
                //  lastOffset = TimeLineOffset;
            }

            var lastAnchor = anchors.Last();
            var month = transactionEvent.Created.Month;
            var day = transactionEvent.Created.Day;
            var hour = transactionEvent.Created.Hour;
            var minute = transactionEvent.Created.Minute;
            var second = transactionEvent.Created.Second;
            //
            //            var overlap = IsOverllap(hour, minute);
            //
            //            if (overlap != null)
            //                return overlap.AddEvent(boxControl.Transaction.Identificator, transactionEvent.Completion, boxControl.HighlightColor);

            //different Day or Month insert separator
            if (lastDay == null || day != lastDay || month != lastMonth)
            {
                lastDay = day;
                lastMonth = month;

                var separator = new TimeLineSeparator()
                {
                    Day = day,
                    Month = month,
                    OffsetX = lastOffset
                };
                //            layout.Children.Add(separator,xConstraint: Constraint.RelativeToParent(p => separatorOffset - item.WidthRequest / 2f - separator.WidthRequest / 2f));
                layout.Children.Add(separator, xConstraint: Constraint.RelativeToParent(p => separator.OffsetX));
                lastOffset += (float)separator.WidthRequest;

                // add event
                var timeEvent = new TimeLineEvent(boxControl.Transaction.Identificator, transactionEvent, boxControl.HighlightColor);
                var item = new HourMinuteAnchor(hour, minute, timeEvent)
                {
                    Completion = transactionEvent.Completion,
                    OffsetX = lastOffset
                };
            }
            //check for spacer
            else
            {
                if(lastAnchor is TimeLineEventAnchor eventAnchor)
                {
                    //add spacer, cos larger space between minutes
                    if (Math.Abs(minute - eventAnchor.Event.Created.Minute) > 1)
                    {
                        lastOffset += 1;
                        var spacer = new AnchorSpacer()
                        {
                            OffsetX = lastOffset
                        };
                        layout.Children.Add(spacer, xConstraint: Constraint.RelativeToParent(p => spacer.OffsetX));

                        lastOffset += (float)spacer.WidthRequest + 1;
                    }
                }               
            }

            //var timeEvent = new TimeLineEvent(boxControl.Transaction.Identificator, transactionEvent, boxControl.HighlightColor);
            //var item = new HourMinuteAnchor(hour, minute, timeEvent)
            //{
            //    Completion = transactionEvent.Completion,
            //    OffsetX = lastOffset
            //};

            //anchors.Add(item);
        }

        private void AddSpacer(float offset)
        {

        }

        private void AddSeparator(double separatorOffset, int day, int month, HourMinuteAnchor item)
        {
            var separator = new TimeLineSeparator()
            {
                Day = day,
                Month = month
            };
            //            layout.Children.Add(separator,xConstraint: Constraint.RelativeToParent(p => separatorOffset - item.WidthRequest / 2f - separator.WidthRequest / 2f));
            layout.Children.Add(separator, xConstraint: Constraint.RelativeToParent(p => separatorOffset));
        }

        //public void PrepareTimeLine(DateTime start, DateTime end)
        //{
        //    anchors = new List<TimeLineAnchor>();
        //    var day = start.Day;
        //    var month = start.Month;

        //    var hour = start.Hour;
        //    var minute = start.Minute;

        //    lastOffset = 0;

        //    AddSeparator(lastOffset, day, month, null);
        //    lastOffset += 6;

        //    while (hour < 24)
        //    {
        //        while (minute != 60)
        //        {
        //            var anchor = new HourMinuteAnchor(hour, minute, new TimeLineEvent("T1", "Rq", Color.Salmon))
        //            {
        //                OffsetX = lastOffset,
        //                Completion = TransactionCompletion.Requested

        //            };
        //            anchors.Add(anchor);
        //            lastOffset += AnchorSpacing;
        //            minute++;
        //        }
        //        hour++;
        //    }

        //    foreach (var anchor in anchors)
        //    {
        //        layout.Children.Add(anchor, xConstraint: Constraint.RelativeToParent(p => anchor.OffsetX));
        //    }
        //}
    }
}