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
	public partial class SecondAnchor : TimeLineEventAnchor
	{

        public static readonly BindableProperty SecondProperty = 
            BindableProperty.Create(nameof(Second), typeof(int),typeof(SecondAnchor), default(int));

        public int Second
        {
            get => (int)GetValue(SecondProperty);
            set { SetValue(SecondProperty, value); OnPropertyChanged(nameof(FormattedSecond)); }
        }

	  

	    public string FormattedSecond => $"{Second}";

//        public SecondAnchor () : base(null)
//		{
//			InitializeComponent ();
//
//		    this.Content.BindingContext = this;
//        }

	    public SecondAnchor(int second, double leftX,  TimeLineEvent timeEvent, TransactionCompletion completion)
        :base(leftX, timeEvent, completion)
	    {
	        InitializeComponent();
	        Second = second;
	        this.Content.BindingContext = this;
        }
    }
}