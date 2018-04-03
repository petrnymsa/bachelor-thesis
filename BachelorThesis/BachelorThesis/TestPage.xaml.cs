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

		    //Content = new Grid()
		    //{
      //          Padding = 20,
		    //    Children =
		    //    {
		    //        new TransactionLinkControl()
		    //        {
		    //            BackgroundColor = Color.PeachPuff,
		    //        }
		    //    }
		    //};
		}
	}
}