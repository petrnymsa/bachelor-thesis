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
	public partial class SecondAnchor : TimeLineAnchor
	{

        public static readonly BindableProperty SecondProperty = 
            BindableProperty.Create(nameof(Second), typeof(int),typeof(SecondAnchor), default(int));

        public int Second
        {
            get => (int)GetValue(SecondProperty);
            set { SetValue(SecondProperty, value); OnPropertyChanged(nameof(FormattedSecond)); }
        }

	  

	    public string FormattedSecond => $":{Second}";

        public SecondAnchor () : base(null)
		{
			InitializeComponent ();

		    this.Content.BindingContext = this;
        }

	    public SecondAnchor(TimeLineEvent timeEvent)
        :base(timeEvent)
	    {
	        InitializeComponent();
	        this.Content.BindingContext = this;
        }
    }
}