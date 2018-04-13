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

        public double TimeLineOffset { get; set; }

        private List<TimeLineItem> items;

        private int? lastDay = null;
        private int? lastMonth = null;

        public TimeLine()
        {
            InitializeComponent();
            items = new List<TimeLineItem>();
        }

        protected TimeLineItem IsOverllap(int hour, int minute)
        {
            return items.FirstOrDefault(x => x.Hour == hour && x.Minute == minute);
        }

        public TimeLineEvent AddEvent(double eventOffset, string identifier, CompletionChangedTransactionEvent transactionEvent, Color color)
        {
            var month = transactionEvent.Created.Month;
            var day = transactionEvent.Created.Day;
            var hour = transactionEvent.Created.Hour;
            var minute = transactionEvent.Created.Minute;

            var overlap = IsOverllap(hour, minute);

            if (overlap != null)
                return overlap.AddEvent(identifier, transactionEvent.Completion, color);


            var item = new TimeLineItem(hour, minute);
            var timeLineEvent = item.AddEvent(identifier, transactionEvent.Completion, color);

            items.Add(item);

            if (lastDay == null)
            {
                lastDay = day;
                lastMonth = month;

                AddSeparator(eventOffset, day, month, item);

            }
            else if (day != lastDay || month != lastMonth)
            {
                lastDay = day;
                lastMonth = month;

                AddSeparator(eventOffset, day, month, item);
            }

            layout.Children.Add(item, xConstraint: Constraint.RelativeToParent(p => TimeLineOffset + eventOffset - item.WidthRequest / 2f));

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