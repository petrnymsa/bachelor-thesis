using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        class Anchor
        {
            public const float AnchorWidth = 8f;

            public float X { get; }
            private bool isFocused;

            public bool IsFocused
            {
                get => isFocused;
                set
                {
                    isFocused = value;
                    AssociatedEvent.IsRevealed = isFocused;
                }
            }

            private TimeLineEvent AssociatedEvent { get; }

            public TransactionCompletion Completion { get; set; }

            public Anchor(float x, TimeLineEvent associatedEvent, TransactionCompletion completion, bool isFocused = false)
            {
                X = x;
                AssociatedEvent = associatedEvent;
                IsFocused = isFocused;
                Completion = completion;
            }

            public bool HitTestX(SKPoint p, float tolerance = 10f)
            {
                return Math.Abs(X - p.X) <= tolerance;
            }
        }

        class BoxDescendant
        {
            public TransactionBoxControl Control { get; set; }
            public TransactionCompletion OffsetCompletion { get; set; }

            public BoxDescendant(TransactionBoxControl control, TransactionCompletion offsetCompletion)
            {
                Control = control;
                OffsetCompletion = offsetCompletion;
            }
        }

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

        public static BindableProperty ProgressColorProperty =
            BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(TransactionLinkControl), Color.Aquamarine,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color ProgressColor
        {
            get => (Color)GetValue(ProgressColorProperty);
            set => SetValue(ProgressColorProperty, value);
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

        public static BindableProperty InvalidColorProperty =
            BindableProperty.Create(nameof(InvalidColor), typeof(Color), typeof(TransactionLinkControl), Color.DarkSalmon,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color InvalidColor
        {
            get => (Color)GetValue(InvalidColorProperty);
            set => SetValue(InvalidColorProperty, value);
        }

        #endregion

        private List<TransactionBoxControl> descendats { get; set; }
        public TransactionInstance Transaction { get; set; }

        private List<TransactionLinkControl> links;

        public TransactionBoxControl()
        {
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Start;

            EnableTouchEvents = true;
            Touch += OnTouch;

            events = new Dictionary<int, List<Anchor>>();
            asociatedEvents = new HashSet<int>();
            descendats = new List<TransactionBoxControl>();
            links = new List<TransactionLinkControl>();
           // IgnorePixelScaling = true;

            //strokeColor = (Color)App.Current.Resources["StrokeColor"];
            //progressColor = (Color)App.Current.Resources["ProgressColor"];
            //stopColor = (Color)App.Current.Resources["StopColor"];

         //   SizeChanged += (sender, args) => InvalidateSurface();
        }

        private readonly Dictionary<int, List<Anchor>> events;
        private readonly HashSet<int> asociatedEvents;

        private SKPoint lastTouch;

        private bool isPressed = false;
        private bool stopped = false;
        private SKImageInfo info;
//        private Color strokeColor;
//        private Color progressColor;
//        private Color stopColor;
        protected float AsPixel(double input)
        {
            var factor = (float)(CanvasSize.Width / WidthRequest);
            return (float)input * factor;

        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            //base.OnPaintSurface(e);

            info = e.Info;
            var canvas = e.Surface.Canvas;

            var formSize = new Size(WidthRequest, HeightRequest);

            var paintBorder = new SKPaint
            {
                IsAntialias = true,
                StrokeWidth = 2.5f,
                Color = MainColor.ToSKColor(),
                Style = SKPaintStyle.Stroke,

            };

            var paintProgress = new SKPaint
            {
                IsAntialias = true,
                StrokeWidth = 1,
                Color = !stopped ? ProgressColor.ToSKColor() : InvalidColor.ToSKColor(),
                Style = SKPaintStyle.StrokeAndFill,

            };
          //  BackgroundColor = Color.LightBlue;
          //  var scaleFactor = (float)(e.Info.Width / this.Width);
            canvas.Clear(Color.LightBlue.ToSKColor());
          //  canvas.Scale(scaleFactor);
            canvas.Save();
            if (!IsActive)
                paintBorder.PathEffect = SKPathEffect.CreateDash(new float[] { 10, 5 }, 1);
            // main rectangle
            canvas.DrawRect(new SKRect(0, 0, (float)AsPixel(WidthRequest), AsPixel(HeightRequest)), paintBorder);
            // progress rectangle
            if (Math.Abs(Progress) > 0.0)
                canvas.DrawRect(new SKRect(3, 3, Progress * info.Width - 2.8f, info.Height - 3), paintProgress);

            paintProgress.Color = Color.Chocolate.ToSKColor();
            paintProgress.StrokeWidth = 2;
           //  progress helpers
            for (int i = 1; i <= 5; i++)
            {
                var cmp = (TransactionCompletion) i;
                canvas.Save();
                canvas.Translate(AsPixel(GetCompletionPositionDPS(cmp)), 0);
                canvas.DrawLine(0, 1, 0, info.Height - 2, paintProgress);
                canvas.Restore();
            }
            canvas.Restore();


            using (SKPaint anchorPaint = new SKPaint())
            {
                anchorPaint.Style = SKPaintStyle.Fill;
                anchorPaint.Color = HighlightColor.ToSKColor();

                foreach (var anchorList in events)
                {
                    foreach (var anchor in anchorList.Value)
                    {
                        var size = anchor.IsFocused ? Anchor.AnchorWidth*2 : Anchor.AnchorWidth;
                        canvas.Save();
                        canvas.Translate(anchor.X, info.Height / 2f);
                        canvas.RotateDegrees(45,0,0);
                        //canvas.DrawCircle(anchor.X, info.Height / 2f, size, paintTouchPoint);
                        canvas.DrawRect(new SKRect(- size / 2f, -size / 2f, size, size), anchorPaint);
                        canvas.Restore();
                    }
                }
            }

        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            //   var lastTouch = SKPoint.Empty;
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    lastTouch = e.Location;
                    isPressed = true;
                    //     touches.Add(lastTouch);
                    break;
                case SKTouchAction.Moved:
                    lastTouch = e.Location;
                    break;
                case SKTouchAction.Released:
                    isPressed = false;
                    break;
            }

            e.Handled = true;

            if (isPressed)
            {
                //foreach (var anchor in anchors)
                //{
                //    var collision = anchor.HitTestX(lastTouch, 20);
                //}

                foreach (var eventAnchors in events)
                {
                    foreach (var anchor in eventAnchors.Value)
                    {
                        var collision = anchor.HitTestX(lastTouch, 20);
                        if (collision)
                        {
                            anchor.IsFocused = true;
                            break;
                        }
                    }
                }
            }
            else events.ForEach(x => x.Value.ForEach(an => an.IsFocused = false));
            
            ((SKCanvasView)sender).InvalidateSurface();
        }

        public void AddProgress(TransactionCompletion completion)
        {
            if (completion == TransactionCompletion.Quitted || completion == TransactionCompletion.Stopped)
                stopped = true;

            var start = Progress;
           // var end = completion.ToPercentValue();
            var end = completion.ToPercentValue();
            this.Animate("ProgressAnimation", x => Progress = (float)x, start, end, 4, 1200, Easing.SinInOut);
        }

        public void AssociateEvent(TimeLineEvent eventControl, TransactionCompletion completion)
        {
           // var percent = completion.ToPercentValue();
            var percent = Progress + 0.2f;
            if (!asociatedEvents.Contains(eventControl.Id))
                asociatedEvents.Add(eventControl.Id);

            if (!events.ContainsKey(eventControl.Id))
                events[eventControl.Id] = new List<Anchor>();

            var x = percent * (float)WidthRequest - Anchor.AnchorWidth / 2f;
            if (completion == TransactionCompletion.Accepted)
                x -= Anchor.AnchorWidth;
            events[eventControl.Id].Add(new Anchor(x, eventControl, completion));

            //    anchors.Add(new Anchor(percent * info.Width, eventControl));
            InvalidateSurface();
        }

        public float GetCompletionPositionDPS(TransactionCompletion completion)
        {
              var percent = completion.ToPercentValue();
           // var percent = Progress + 0.2f;
            return ((float) WidthRequest * percent); // - TransactionLinkControl.ShapeRadius;//*2 - TransactionLinkControl.ShapeRadius/2f;
        }

        public float GetCompletionOffset(TransactionCompletion completion)
        {
            return GetCompletionPositionDPS(completion) + TransactionLinkControl.StateToRequestArrowHead + TransactionLinkControl.ShapeRadius;
        }

        public void AddDescendant(TransactionBoxControl descendant)
        {
            descendant.SizeChanged += (sender, args) =>
            {
                var box = (TransactionBoxControl) sender;
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

        
    }
}
