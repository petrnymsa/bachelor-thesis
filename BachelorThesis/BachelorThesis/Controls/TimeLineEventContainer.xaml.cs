using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimeLineEventContainer : ContentView
	{
        public static readonly BindableProperty EventProperty = BindableProperty.Create(nameof(Event), typeof(TimeLineEvent), typeof(TimeLineEventContainer), default(TimeLineEvent));

        public TimeLineEvent Event
        {
            get => (TimeLineEvent)GetValue(EventProperty);
            set => SetValue(EventProperty, value);
        }

	    public TimeLineEventContainer()
	    {
	        InitializeComponent ();

	        this.Content.BindingContext = this;
	    }
        public TimeLineEventContainer (TimeLineEvent timeEvent)
	    {
	        InitializeComponent ();
	        Event = timeEvent;

	        this.Content.BindingContext = this;

        }
    }
}