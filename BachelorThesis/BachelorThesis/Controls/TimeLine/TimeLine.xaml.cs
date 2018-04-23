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
        public const float HourMinuteSpacing = 60;
        // public float TimeLineOffset { get; set; }

        //    private List<HourMinuteAnchor> items;

        private int? lastDay = null;
        private int? lastMonth = null;

        Random rnd = new Random();

        private List<TimeLineAnchor> anchors;
        private double currentX = 0;

        //private double CurrentX =

        //   private bool initialized = false;
        public TimeLine()
        {
            InitializeComponent();
            //   items = new List<HourMinuteAnchor>();
            anchors = new List<TimeLineAnchor>();

            BackgroundColor = Color.LightGreen;
        }

        //protected HourMinuteAnchor IsOverllap(int hour, int minute)
        //{
        //    return anchors.FirstOrDefault(x => x.Hour == hour && x.Minute == minute);
        //}

        public void AssociateEvent(TransactionBoxControl boxControl, TransactionEvent transactionEvent)
        {
            //            if (!initialized)
            //            {
            //                initialized = true;
            //                //  currentX = TimeLineOffset;
            //            }

            var lastAnchor = anchors.LastOrDefault();
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

                AddDayMonthSeparator(month, day);

                //and add event
                AddHourMinuteAnchor(boxControl, transactionEvent, hour, minute);

                return;
            }

            if (lastAnchor != null && lastAnchor is TimeLineEventAnchor eventAnchor)
            {

                // add seconds
                if (minute == eventAnchor.Event.Created.Minute)
                {
                    AddSecondsAnchor(boxControl, transactionEvent, second);
                }
                else // add spacer
                {
                    if (!(eventAnchor is SecondAnchor))
                    {
                        AddSpacer();
                    }
                    else
                    {

                    }

                    AddHourMinuteAnchor(boxControl, transactionEvent, hour, minute);
                }
            }
            else // not event anchor, put new HourMinute
            {
                AddHourMinuteAnchor(boxControl, transactionEvent, hour, minute);
            }
        }

        private void AddDayMonthSeparator(int month, int day)
        {
            var separator = new TimeLineSeparator(day, month, currentX); // {BackgroundColor = Color.Aquamarine};
                                                                         //            layout.Children.Add(separator,xConstraint: Constraint.RelativeToParent(p => separatorOffset - item.WidthRequest / 2f - separator.WidthRequest / 2f));
            layout.Children.Add(separator, xConstraint: Constraint.RelativeToParent(p => separator.LeftX));
            currentX += (float)separator.Width;
        }

        private void AddSpacer()
        {
            currentX += 1;
            var spacer = new AnchorSpacer(currentX); // {BackgroundColor = Color.GreenYellow};
            anchors.Add(spacer);

            layout.Children.Add(spacer,
                xConstraint: Constraint.RelativeToParent(p => spacer.LeftX),
                yConstraint: Constraint.RelativeToParent(p => p.Height * 0.5f - spacer.Height / 2f));

            currentX += (float)spacer.Width + 1;
        }

        private void AddSecondsAnchor(TransactionBoxControl boxControl, TransactionEvent transactionEvent, int second)
        {
            var timeEvent = new TimeLineEvent(boxControl.Transaction.Identificator, transactionEvent, boxControl.HighlightColor);
            var item = new SecondAnchor(second, currentX, timeEvent, transactionEvent.Completion)
            {
                BackgroundColor = Color.FromRgb(rnd.Next(150, 200), rnd.Next(150, 200), rnd.Next(150, 200))
            };

            anchors.Add(item);
            layout.Children.Add(item,
                xConstraint: Constraint.RelativeToParent(p => item.LeftX),
                yConstraint: Constraint.RelativeToParent(p => p.Height * 0.5 - item.Height * 0.5));

            currentX += (float)item.Width;
        }

        private void AddHourMinuteAnchor(TransactionBoxControl boxControl, TransactionEvent transactionEvent, int hour, int minute)
        {
            var timeEvent =
                new TimeLineEvent(boxControl.Transaction.Identificator, transactionEvent, boxControl.HighlightColor);
            var item = new HourMinuteAnchor(hour, minute, currentX, timeEvent, transactionEvent.Completion)
            {
               // BackgroundColor = Color.SandyBrown
            };
            anchors.Add(item);
            layout.Children.Add(item, xConstraint: Constraint.RelativeToParent(p => item.LeftX));

            currentX += (float)item.Width;
        }

        //   private void AddSeparator(double separatorOffset, int day, int month, HourMinuteAnchor item)
        //        {
        //            var separator = new TimeLineSeparator()
        //            {
        //                Day = day,
        //                Month = month
        //            };
        //            //            layout.Children.Add(separator,xConstraint: Constraint.RelativeToParent(p => separatorOffset - item.WidthRequest / 2f - separator.WidthRequest / 2f));
        //            layout.Children.Add(separator, xConstraint: Constraint.RelativeToParent(p => separatorOffset));
        //        }

        //public void PrepareTimeLine(DateTime start, DateTime end)
        //{
        //    anchors = new List<TimeLineAnchor>();
        //    var day = start.Day;
        //    var month = start.Month;

        //    var hour = start.Hour;
        //    var minute = start.Minute;

        //    currentX = 0;

        //    AddSeparator(currentX, day, month, null);
        //    currentX += 6;

        //    while (hour < 24)
        //    {
        //        while (minute != 60)
        //        {
        //            var anchor = new HourMinuteAnchor(hour, minute, new TimeLineEvent("T1", "Rq", Color.Salmon))
        //            {
        //                LeftX = currentX,
        //                Completion = TransactionCompletion.Requested

        //            };
        //            anchors.Add(anchor);
        //            currentX += HourMinuteSpacing;
        //            minute++;
        //        }
        //        hour++;
        //    }

        //    foreach (var anchor in anchors)
        //    {
        //        layout.Children.Add(anchor, xConstraint: Constraint.RelativeToParent(p => anchor.LeftX));
        //    }
        //}
    }
}