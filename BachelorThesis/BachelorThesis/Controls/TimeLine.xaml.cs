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
	    private List<TimeLineItem> items;

	    private int? lastDay = null;
        private int? lastMonth = null;

		public TimeLine ()
		{
			InitializeComponent ();

            items = new List<TimeLineItem>();
		}

	    protected TimeLineItem IsOverllap(int hour, int minute)
	    {
	        return items.FirstOrDefault(x => x.Hour == hour && x.Minute == minute);
	    }

	    public void AddEvent(double offset,string identifier, CompletionChangedTransactionEvent transactionEvent, Color color)
	    {
            //todo get offset for layout
	        var month = transactionEvent.Created.Month;
	        var day = transactionEvent.Created.Day;
	        var hour = transactionEvent.Created.Hour;
	        var minute = transactionEvent.Created.Minute;

	        if (lastDay == null)
	        {
	            lastDay = day;
	            lastMonth = month;
	        }
            else if (day != lastDay || month != lastMonth)
	        {
                //todo add separator 

	        }

	        var overlap = IsOverllap(hour, minute);

	        if (overlap == null)
	        {
                var item = new TimeLineItem(hour, minute);
                item.AddEvent(identifier, transactionEvent.Completion, color);

                items.Add(item);

                //todo add TimeLineItem to layout with offset

                layout.Children.Add(item,xConstraint: Constraint.RelativeToParent(p => offset));
	        }
	        else
	        {
                overlap.AddEvent(identifier, transactionEvent.Completion,color);
	        }
	    }
	}
}