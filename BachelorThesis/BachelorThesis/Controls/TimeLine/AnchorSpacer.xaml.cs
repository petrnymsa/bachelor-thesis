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
	public partial class AnchorSpacer : TimeLineAnchor
	{
		public AnchorSpacer (double leftX) : base(leftX)
		{
			InitializeComponent ();
		}
	}
}