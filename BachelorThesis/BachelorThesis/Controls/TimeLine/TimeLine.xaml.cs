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
        private const int FullTimeSpacing = 6;
        private int? lastDay = null;
        private int? lastMonth = null;

        private List<TimeLineAnchor> anchors;
        private double currentX = 0;
        public TimeLine()
        {
            InitializeComponent();
            //   items = new List<HourMinuteAnchor>();
            anchors = new List<TimeLineAnchor>();
        }
        public void AssociateEvent(TransactionBoxControl boxControl, TransactionEvent transactionEvent)
        {

            var lastAnchor = anchors.LastOrDefault();

            var month = transactionEvent.Created.Month;
            var day = transactionEvent.Created.Day;
            var hour = transactionEvent.Created.Hour;
            var minute = transactionEvent.Created.Minute;
            var second = transactionEvent.Created.Second;

            //different Day or Month insert separator
            if (lastDay == null || day != lastDay || month != lastMonth)
            {
                lastDay = day;
                lastMonth = month;

                AddDayMonthSeparator(month, day);

                //and add event
                // AddHourMinuteAnchor(boxControl, transactionEvent, hour, minute);
                AddTimeAnchor(hour, minute, second, boxControl, transactionEvent);

                return;
            }

            if (lastAnchor != null && lastAnchor is TimeLineEventAnchor eventAnchor)
            {
                if (Math.Abs(minute - eventAnchor.Event.Created.Minute) > 1)
                {
                    AddSpacer(eventAnchor);
                }
                AddTimeAnchor(hour, minute, second, boxControl, transactionEvent);
            }
            else // not event anchor, put new Time
            {
                AddTimeAnchor(hour, minute, second, boxControl, transactionEvent);
            }
        }

        private void AddTimeAnchor(int hour, int minute, int second, TransactionBoxControl boxControl,
            TransactionEvent transactionEvent)
        {
            var timeEvent = new TimeLineEvent(boxControl.Transaction.Identificator, transactionEvent, boxControl.HighlightColor);
            var anchor = new FullTimeAnchor(hour, minute, second, currentX, timeEvent, transactionEvent.Completion);
            anchors.Add(anchor);

            layout.Children.Add(anchor, xConstraint: Constraint.RelativeToParent(p => anchor.LeftX));
            scrollView.ScrollToAsync(anchor, ScrollToPosition.End, true);

            currentX += (float)anchor.Width + FullTimeSpacing;
        }

        private void AddTimeAnchorWithoutEvent(int hour, int minute, int second)
        {
            // var timeEvent = new TimeLineEvent(boxControl.Transaction.Identificator, transactionEvent, boxControl.HighlightColor);
            var anchor = new FullTimeAnchor(hour, minute, second, currentX, null, TransactionCompletion.None)
            {
                WithEvent = false
            };
            anchors.Add(anchor);

            layout.Children.Add(anchor, xConstraint: Constraint.RelativeToParent(p => anchor.LeftX));

            currentX += (float)anchor.Width + FullTimeSpacing;
        }

        private void AddDayMonthSeparator(int month, int day)
        {

            var separator = new TimeLineSeparator(day, month, currentX);
            layout.Children.Add(separator, xConstraint: Constraint.RelativeToParent(p => separator.LeftX));
            scrollView.ScrollToAsync(separator, ScrollToPosition.End, true);

            currentX += (float)separator.Width;
        }

        private void AddSpacer(View view)
        {
            // currentX += 1;
            var spacer = new AnchorSpacer(currentX); // {BackgroundColor = Color.GreenYellow};
            anchors.Add(spacer);

            layout.Children.Add(spacer,
                xConstraint: Constraint.RelativeToParent(p => spacer.LeftX),
                yConstraint: Constraint.RelativeToParent(p => 4));
            //      yConstraint: Constraint.RelativeToParent(p => p.Height * 0.5f - spacer.Height / 2f));
            scrollView.ScrollToAsync(spacer, ScrollToPosition.End, true);
            currentX += (float)spacer.Width + FullTimeSpacing;
        }

        private void AddSecondsAnchor(TransactionBoxControl boxControl, TransactionEvent transactionEvent, int second)
        {
            var timeEvent = new TimeLineEvent(boxControl.Transaction.Identificator, transactionEvent, boxControl.HighlightColor);
            var item = new SecondAnchor(second, currentX, timeEvent, transactionEvent.Completion)
            {
                //  BackgroundColor = Color.FromRgb(rnd.Next(150, 200), rnd.Next(150, 200), rnd.Next(150, 200))
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

        private void AddHourMinuteAnchorWithoutEvent(int hour, int minute)
        {
            var item = new HourMinuteAnchor(hour, minute, currentX, null, TransactionCompletion.None)
            {
                WithEvent = false
                // BackgroundColor = Color.SandyBrown
            };
            anchors.Add(item);
            layout.Children.Add(item, xConstraint: Constraint.RelativeToParent(p => item.LeftX));

            currentX += (float)item.Width;
        }

        public void Reset()
        {
            anchors.Clear();
            currentX = 0;
            lastDay = lastMonth = null;

            layout.Children.Clear();
        }
    }
}