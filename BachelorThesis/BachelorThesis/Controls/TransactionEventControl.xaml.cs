using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionEventControl : ContentView
    {
        //public static readonly BindableProperty TransactionIdentifierProperty =
        //    BindableProperty.Create(nameof(TransactionIdentifier), typeof(string), typeof(TransactionEventControl), string.Empty, 
        //       BindingMode.TwoWay, propertyChanged: (b, o, n) => {    });

        //public string TransactionIdentifier
        //{
        //    get => (string)GetValue(TransactionIdentifierProperty);
        //    set => SetValue(TransactionIdentifierProperty, value);
        //}


        public static readonly BindableProperty TopLabelProperty = BindableProperty.Create(nameof(TopLabel), typeof(string), typeof(TransactionEventControl), default(string));

        public string TopLabel
        {
            get => (string)GetValue(TopLabelProperty);
            set => SetValue(TopLabelProperty, value);
        }

        public static readonly BindableProperty BottomLabelProperty = BindableProperty.Create(nameof(BottomLabel), typeof(string), typeof(TransactionEventControl), default(string));

        public string BottomLabel
        {
            get => (string)GetValue(BottomLabelProperty);
            set => SetValue(BottomLabelProperty, value);
        }


        public TransactionEventControl ()
		{
			InitializeComponent ();

            

		    //this.Content = new StackLayout()
		    //{
		    //    Children =
		    //    {
		    //        new Label() {Text = "T1[pm]"},
		    //        new BoxView() {Color = Color.LightBlue, WidthRequest = 20, HorizontalOptions = LayoutOptions.Center},
		    //        new Label() {Text = "8.4"},
		    //        new Label() {Text = "14:12:42"}
		    //    }
		    //};
		}
	}
}