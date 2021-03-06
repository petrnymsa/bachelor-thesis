﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimeLineSeparator : TimeLineAnchor
	{
	    public static readonly BindableProperty DayProperty = BindableProperty.Create(nameof(Day), typeof(int), typeof(HourMinuteAnchor), 0);

	    public int Day
	    {
	        get => (int)GetValue(DayProperty);
	        set { SetValue(DayProperty, value); OnPropertyChanged(nameof(FormattedString)); }
	    }


	    public static readonly BindableProperty MonthProperty = BindableProperty.Create(nameof(Month), typeof(int), typeof(HourMinuteAnchor), 0);

	    public int Month
	    {
	        get => (int)GetValue(MonthProperty);
	        set { SetValue(MonthProperty, value); OnPropertyChanged(nameof(FormattedString)); }
	    }

	    public string FormattedString => $"{Day}. {Month}.";

        public TimeLineSeparator (int day, int month, double leftX) : base(leftX)
		{
			InitializeComponent ();

		    Day = day;
		    Month = month;
		}
	}
}