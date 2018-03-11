using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BachelorThesis.Controls;
using BachelorThesis.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ProgressBar = Xamarin.Forms.ProgressBar;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarDroidRenderer))]
namespace BachelorThesis.Droid
{
    public class CustomProgressBarDroidRenderer : ProgressBarRenderer
    {
        public CustomProgressBarDroidRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.CornflowerBlue.ToAndroid()); //Change the color
                
         //       Control.ProgressDrawable.SetColorFilter(Color.FromRgb(182, 231, 233).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);
                //Control.ProgressTintListColor.FromRgb(182, 231, 233).ToAndroid();
           //     Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.FromRgb(182, 231, 233).ToAndroid());

                var progressBar = e.NewElement as CustomProgressBar;

                Control.ScaleY = progressBar?.BarHeight ?? 10; //Changes the height

                
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender,e);
            if(!(sender is CustomProgressBar progressBar))
                return;

            if (e.PropertyName == CustomProgressBar.BarHeightProperty.PropertyName)
                System.Diagnostics.Debug.WriteLine(Control.ScaleY);
            if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName)
            {
                if(Math.Abs(progressBar.Progress - 1) < 1E-12)
                    Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.ForestGreen.ToAndroid()); //Change the color
                else Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.CornflowerBlue.ToAndroid());

            }
        }
    }
}