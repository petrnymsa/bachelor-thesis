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

            //Content = new StackLayout()
            //{
            //    BackgroundColor = Color.Azure,
            //    Padding = 20,
            //    Children =
            //    {
            //        new TransactionBoxControl()
            //        {
            //            WidthRequest = 200,
            //            HeightRequest = 30,
            //            BackgroundColor = Color.DarkRed,
            //            HorizontalOptions = LayoutOptions.Start,
            //            VerticalOptions = LayoutOptions.Start,
            //            Progress = 0.5f
            //        }
            //    }
            //};
        }

	    private void Button_OnClicked(object sender, EventArgs e)
	    {
	       // Progress1.Animate(nameof(Progress1), x => Progress1.EndingDegrees = (float)x, 0, endingAt1, 4, 5000, Easing.BounceIn);

	        var start = boxControl.Progress;
            boxControl.Animate("ProgrAnim",x=> boxControl.Progress = (float)x,start, 1f,4,2000,Easing.Linear);

	     //   boxControl.Animate("ProgrAnim", x => boxControl.Progress = (float)x, start, 0.0f, 4, 2000, Easing.Linear);

        }
    }
}