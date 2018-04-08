using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class TransactionBoxControl : SKCanvasView
    {
        public static BindableProperty ProgressProperty =
            BindableProperty.Create(nameof(Progress), typeof(float), typeof(TransactionLinkControl), 0.0f,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public float Progress
        {
            get => (float)GetValue(ProgressProperty);
            set
            {
                IsActive = value > 0;

                SetValue(ProgressProperty, value);
            }
        }

        public static BindableProperty IsActiveProperty =
            BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(TransactionLinkControl), true,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public TransactionBoxControl()
        {
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Start;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var paintBorder = new SKPaint
            {
                StrokeWidth = 1,
                Color = Color.Black.ToSKColor(),
                Style = SKPaintStyle.Stroke,

            };

            var paintProgress = new SKPaint
            {
                StrokeWidth = 1,
                Color = Color.LightGreen.ToSKColor(),
                Style = SKPaintStyle.StrokeAndFill,

            };

            var width = (float)WidthRequest;
            var height = (float)HeightRequest;
            var progressWidth = width / 100f;

            var canvas = e.Surface.Canvas;

            canvas.Clear();
            canvas.Scale((float)(e.Info.Width / this.Width));

            if (!IsActive)
                paintBorder.PathEffect = SKPathEffect.CreateDash(new float[] {10,5},1);

            canvas.DrawRect(new SKRect(0, 0, width-1, height-1), paintBorder);
            
            if (Math.Abs(Progress) > 0.0) 
                canvas.DrawRect(new SKRect(1, 1, Progress * 100 * progressWidth - 2, height - 2), paintProgress);
          //  Debug.WriteLine($"Progress: {Progress}");
           // canvas.DrawText(progressWidth.ToString(),60,60, paintText);
         
            paintBorder.Dispose();
            paintProgress.Dispose();
          
        }
    }
}
