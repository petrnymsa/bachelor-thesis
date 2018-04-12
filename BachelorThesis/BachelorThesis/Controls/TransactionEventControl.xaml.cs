using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionEventControl : ContentView
    {
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

        public static readonly BindableProperty IsRevealedProperty = BindableProperty.Create(nameof(IsRevealed), typeof(bool), 
            typeof(TransactionEventControl), false, propertyChanged: (bindable, old, newValue) =>
            {

            });

        public bool IsRevealed
        {
            get => (bool)GetValue(IsRevealedProperty);
            set => SetValue(IsRevealedProperty, value);
        }

        public TransactionCompletion Completion { get; set; }


        public TransactionEventControl ()
		{
			InitializeComponent ();

            this.SizeChanged += OnSizeChanged;
		}

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {

        //    var height = (double)GetValue(He);

            DebugHelper.Info($"IsRevealedChanged = {IsRevealed}");
            DebugHelper.Info($"Height = {Height}");
            RelativeLayout.SetYConstraint(this, Constraint.RelativeToParent(p => p.Height * 0.5f - Height / 2f));
        }
    }
}