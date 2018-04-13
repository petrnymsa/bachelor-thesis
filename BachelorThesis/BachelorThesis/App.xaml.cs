using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BachelorThesis.Views;
using Xamarin.Forms;

namespace BachelorThesis
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
		    MainPage = new MainPage();
		    //   MainPage = new NavigationPage(new MainPage());
		    //MainPage = new TestPage();
		    //   ((NavigationPage)MainPage).BarBackgroundColor = Color.FromHex()


		}

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
