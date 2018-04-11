using System;
using System.Diagnostics;
using BachelorThesis.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;

namespace BachelorThesis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{


        public TestPage ()
		{
			InitializeComponent ();
        }

	    private void Button_OnClicked(object sender, EventArgs e)
	    {
	        eventControl.TransactionIdentifier = transactionId.Text;
	    }
	}
}