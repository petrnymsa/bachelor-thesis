using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BachelorThesis.Controls
{
    public class TransactionBoxControl : SKCanvasView
    {
        //class Anchor
        //{
        //    public const float AnchorWidth = 8f;

        //    public float X { get; }
        //    private bool isFocused;

        //    public bool IsFocused
        //    {
        //        get => isFocused;
        //        set
        //        {
        //            isFocused = value;
        //          //  AssociatedEvent.IsRevealed = isFocused;
        //        }
        //    }

        //    private TimeLineAnchor AssociatedEvent { get; }

        //    public Anchor(float x, TimeLineAnchor associatedEvent, bool isFocused = false)
        //    {
        //        X = x;
        //        AssociatedEvent = associatedEvent;
        //        IsFocused = isFocused;

        //    }

        //    public bool HitTestX(SKPoint p, float tolerance = 10f)
        //    {
        //        return Math.Abs(X - p.X) <= tolerance;
        //    }
        //}

        //class BoxDescendant
        //{
        //    public TransactionBoxControl Control { get; set; }
        //    public TransactionCompletion OffsetCompletion { get; set; }

        //    public BoxDescendant(TransactionBoxControl control, TransactionCompletion offsetCompletion)
        //    {
        //        Control = control;
        //        OffsetCompletion = offsetCompletion;
        //    }
        //}

        #region BindableProperties
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
                (bindable, oldValue, newValue) =>
                {
                    if (oldValue != newValue)
                        (bindable as TransactionBoxControl).InvalidateSurface();
                });

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public static BindableProperty TransactionIdProperty =
            BindableProperty.Create(nameof(TransactionId), typeof(int?), typeof(TransactionLinkControl));

        public int? TransactionId
        {
            get => (int?)GetValue(TransactionIdProperty);
            set => SetValue(TransactionIdProperty, value);
        }

        public static BindableProperty HighlightColorProperty =
            BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(TransactionLinkControl), Color.Red,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color HighlightColor
        {
            get => (Color)GetValue(HighlightColorProperty);
            set => SetValue(HighlightColorProperty, value);
        }
        public static BindableProperty MainColorProperty =
            BindableProperty.Create(nameof(MainColor), typeof(Color), typeof(TransactionLinkControl), Color.Black,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color MainColor
        {
            get => (Color)GetValue(MainColorProperty);
            set => SetValue(MainColorProperty, value);
        }

        public static BindableProperty ProgressColorProperty =
            BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(TransactionLinkControl), Color.LimeGreen,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color ProgressColor
        {
            get => (Color)GetValue(ProgressColorProperty);
            set => SetValue(ProgressColorProperty, value);
        }


        public static BindableProperty StopColorProperty =
            BindableProperty.Create(nameof(StopColor), typeof(Color), typeof(TransactionLinkControl), Color.Red,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color StopColor
        {
            get => (Color)GetValue(StopColorProperty);
            set => SetValue(StopColorProperty, value);
        }


        public static BindableProperty WarninigColorProperty =
            BindableProperty.Create(nameof(WarningColor), typeof(Color), typeof(TransactionLinkControl), Color.Yellow,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color WarningColor
        {
            get => (Color)GetValue(WarninigColorProperty);
            set => SetValue(WarninigColorProperty, value);
        }

        public TransactionInstance Transaction { get; set; }
        public TransactionBoxControl ParentControl { get; set; }

        #endregion

        private bool stopped = false;
        private TransactionCompletion currentCompletion = TransactionCompletion.None;
        private const float BorderWidth = 3f;
        private readonly List<TransactionBoxControl> descendats;
        private readonly List<TransactionLinkControl> links;


        public TransactionBoxControl()
        {
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Start;


            descendats = new List<TransactionBoxControl>();
            links = new List<TransactionLinkControl>();
        }

        protected float AsPixel(double input)
        {
            var factor = (float)(CanvasSize.Width / WidthRequest);
            return (float)input * factor;

        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            //base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;

            var paintBorder = new SKPaint
            {
                IsAntialias = true,
                StrokeWidth = BorderWidth,
                Color = MainColor.ToSKColor(),
                Style = SKPaintStyle.Stroke,

            };

            var progressColor = ProgressColor;
            if (currentCompletion == TransactionCompletion.Declined || currentCompletion == TransactionCompletion.Rejected)
                progressColor = WarningColor;
            else if (currentCompletion == TransactionCompletion.Stopped || currentCompletion == TransactionCompletion.Quitted)
                progressColor = StopColor;

            var paintProgress = new SKPaint
            {
                IsAntialias = true,
                StrokeWidth = 1,
                Color = progressColor.ToSKColor(),
                Style = SKPaintStyle.StrokeAndFill,

            };
            //  BackgroundColor = Color.LightBlue;
            //  var scaleFactor = (float)(e.Info.Width / this.Width);
            canvas.Clear();
            //  canvas.Scale(scaleFactor);
            //  canvas.Save();
            if (!IsActive)
                paintBorder.PathEffect = SKPathEffect.CreateDash(new float[] { 10, 5 }, 1);
            // main rectangle
            canvas.DrawRect(new SKRect(0, 0, AsPixel(WidthRequest), AsPixel(HeightRequest)), paintBorder);
            // progress rectangle
            if (Math.Abs(Progress) > 0.0)
                canvas.DrawRect(new SKRect(BorderWidth, BorderWidth, AsPixel(Progress * WidthRequest) - BorderWidth, AsPixel(HeightRequest) - BorderWidth), paintProgress);

            //  progress helpers
            //            paintProgress.Color = Color.Chocolate.ToSKColor();
            //            paintProgress.StrokeWidth = 2;
            //            for (int i = 1; i <= 5; i++)
            //            {
            //                var cmp = (TransactionCompletion)i;
            //                canvas.Save();
            //                canvas.Translate(AsPixel(GetCompletionPosition(cmp)), 0);
            //                canvas.DrawLine(0, 1, 0, info.Height - 2, paintProgress);
            //                canvas.Restore();
            //            }
            //            canvas.Restore();

        }

        public void AddProgress(TransactionCompletion completion)
        {
            currentCompletion = completion;
            if (completion == TransactionCompletion.Quitted || completion == TransactionCompletion.Stopped)
                stopped = true;

            var start = Progress;
            // var end = completion.ToPercentValue();
            var end = completion.ToPercentValue();
            this.Animate("ProgressAnimation", x => Progress = (float)x, start, end, 4, 1200, Easing.SinInOut);

            if(Math.Abs(start - end) < 0.000001)
                InvalidateSurface();
            
        }


        public float GetCompletionPosition(TransactionCompletion completion)
        {
            var percent = completion.ToPercentValue();
            return ((float)WidthRequest * percent); // - TransactionLinkControl.ShapeRadius;//*2 - TransactionLinkControl.ShapeRadius/2f;
        }

        public float GetCompletionOffset(TransactionCompletion completion)
        {
            return GetCompletionPosition(completion) + TransactionLinkControl.StateToRequestArrowHead + TransactionLinkControl.ShapeRadius;
        }

        public void AddDescendant(TransactionBoxControl descendant)
        {
            descendant.SizeChanged += (sender, args) =>
            {
                var box = (TransactionBoxControl)sender;
                if (box.WidthRequest > WidthRequest - GetCompletionOffset(TransactionCompletion.Requested))
                {
                    WidthRequest = box.WidthRequest + GetCompletionOffset(TransactionCompletion.Requested);
                    InvalidateSurface();

                    links.ForEach(x => x.RefreshLayout());
                }
            };

            descendats.Add(descendant);
        }

        public void AddLink(TransactionLinkControl link)
        {
            links.Add(link);
        }

        public void ResetProgress()
        {
            Progress = 0;
            InvalidateSurface();
            
        }
    }
}
