using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SimulationCasesPage : ContentPage
	{
		public SimulationCasesPage ()
		{
			InitializeComponent ();

           this.Content.BindingContext = new SimulationCasesPageViewModel(Navigation);
		}
	}
}