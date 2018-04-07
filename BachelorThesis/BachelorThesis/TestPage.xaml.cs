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

            Content = new Grid()
            {
                BackgroundColor = Color.Azure,
                Padding = 20,
                Children =
                {
                    new BoxView()
                    {
                        WidthRequest = 80,
                        HeightRequest = 80,
                        BackgroundColor = Color.LightGreen,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start
                    },
                    new TransactionLinkControl()
                    {
                       BackgroundColor = Color.Transparent,
                        SourceText = "Rq",
                        TargetText = "Pm"
                    }


                }
            };
        }
	}
}