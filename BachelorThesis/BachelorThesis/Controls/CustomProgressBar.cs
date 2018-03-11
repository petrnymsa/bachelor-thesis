using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class CustomProgressBar : ProgressBar
    {
        public static readonly BindableProperty BarHeightProperty = BindableProperty.Create("BarHeight", typeof(int), typeof(CustomProgressBar), 10);

        public int BarHeight
        {
            get => (int)GetValue(BarHeightProperty);
            set => SetValue(BarHeightProperty, value);
        }
    }
}
