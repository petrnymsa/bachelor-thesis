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
	public partial class FullTimeAnchor : TimeLineEventAnchor
    {
	    public static readonly BindableProperty HourProperty = BindableProperty.Create(nameof(Hour), typeof(int), typeof(FullTimeAnchor), 0);

	    public int Hour
	    {
	        get => (int)GetValue(HourProperty);
	        set { SetValue(HourProperty, value); OnPropertyChanged(nameof(FormattedString)); }
	    }


	    public static readonly BindableProperty MinuteProperty = BindableProperty.Create(nameof(Minute), typeof(int), typeof(FullTimeAnchor), 0);

	    public int Minute
	    {
	        get => (int) GetValue(MinuteProperty);
	        set
	        {
	            SetValue(MinuteProperty, value);
	            OnPropertyChanged(nameof(FormattedString));
	        }

	    }

	    public static readonly BindableProperty SecondProperty =
	        BindableProperty.Create(nameof(Seconds), typeof(int), typeof(SecondAnchor), default(int));

	    public int Seconds
	    {
	        get => (int)GetValue(SecondProperty);
	        set { SetValue(SecondProperty, value); OnPropertyChanged(nameof(FormattedString)); }
	    }

	    public string FormattedString => $"{Hour:00}:{Minute:00}:{Seconds:00}";

        public FullTimeAnchor (int hour, int minute, int seconds, double leftX, TimeLineEvent timeEvent, TransactionCompletion completion)
        :base(leftX, timeEvent, completion)
		{
			InitializeComponent ();

		    Hour = hour;
		    Minute = minute;
		    Seconds = seconds;
		}
	}
}