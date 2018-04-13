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

            public Anchor(float x, TimeLineEvent associatedEvent, bool isFocused = false)
            {
                X = x;
                AssociatedEvent = associatedEvent;
                IsFocused = isFocused;
            }

            public bool HitTestX(SKPoint p, float tolerance = 10f)
            {
                return Math.Abs(X - p.X) <= tolerance;
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
            BindableProperty.Create(nameof(Progress), typeof(int?), typeof(TransactionLinkControl), null,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public int? TransactionId
        {
            get => (int?)GetValue(TransactionIdProperty);
            set => SetValue(TransactionIdProperty, value);
        }

        public static BindableProperty HighlightColorProperty =
            BindableProperty.Create(nameof(Progress), typeof(Color), typeof(TransactionLinkControl), Color.Red,
                BindingMode.OneWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionBoxControl).InvalidateSurface(); });

        public Color HighlightColor
        {
            get => (Color)GetValue(HighlightColorProperty);
            set => SetValue(HighlightColorProperty, value);
        }

        #endregion

        public TransactionBoxControl()
        {
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Start;

            EnableTouchEvents = true;
            Touch += OnTouch;

          // anchors = new List<Anchor>();
            events = new Dictionary<int, List<Anchor>>();
            asociatedEvents = new HashSet<int>();
        }

       // private readonly List<Anchor> anchors;

        private Dictionary<int, List<Anchor>> events;
        private HashSet<int> asociatedEvents;

        private SKPoint lastTouch;

        private bool isPressed = false;
        private SKImageInfo info;


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

            // update the Canvas as you wish
            ((SKCanvasView)sender).InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            //base.OnPaintSurface(e);

            info = e.Info;
            var canvas = e.Surface.Canvas;

            
            var paintBorder = new SKPaint
            {
                IsAntialias = true,
                StrokeWidth = 1,
                Color = Color.Black.ToSKColor(),
                Style = SKPaintStyle.Stroke,

            };

            var paintProgress = new SKPaint
            {
                IsAntialias = true,
                StrokeWidth = 1,
                Color = SKColor.Parse("#8BC34A"),
                Style = SKPaintStyle.StrokeAndFill,

            };

            // var scaleFactor = (float)(e.Info.Width / this.Width);
            canvas.Clear();
            //   canvas.Scale(scaleFactor);
            canvas.Save();
            if (!IsActive)
                paintBorder.PathEffect = SKPathEffect.CreateDash(new float[] { 10, 5 }, 1);
            // main rectangle
            canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paintBorder);
            // progress rectangle
            if (Math.Abs(Progress) > 0.0)
                canvas.DrawRect(new SKRect(1, 1, Progress * info.Width - 2f, info.Height - 1), paintProgress);

            paintProgress.Color = Color.Chocolate.ToSKColor();
            paintProgress.StrokeWidth = 2;
            // progress helpers
            //for (int i = 0; i < 5; i++)
            //{
            //    canvas.Translate(0.2f * width, 0);
            //    canvas.DrawLine(0, 1, 0, height - 2, paintProgress);
            //}
            canvas.Restore();


            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Fill;
                paintTouchPoint.Color = HighlightColor.ToSKColor();
               
                //foreach (var anchor in anchors)
                //    canvas.DrawCircle(anchor.X, info.Height / 2f, anchor.IsFocused ? 16 : 8, paintTouchPoint);

                foreach (var anchorList in events)
                {
                    foreach (var anchor in anchorList.Value)
                        canvas.DrawCircle(anchor.X, info.Height / 2f, anchor.IsFocused ? 16 : 8, paintTouchPoint);
                }

                //if (isPressed)
                //{
                //    paintTouchPoint.Color = SKColors.Black;
                //    canvas.DrawLine(lastTouch.X, 0, lastTouch.X, e.Info.Height, paintTouchPoint);
                //}
            }

            //  paintBorder.Dispose();
            // paintProgress.Dispose();

        }

        public void AddProgress(TransactionCompletion completion)
        {
            var start = Progress;
            var end = completion.ToPercentValue();
            this.Animate("ProgressAnimation", x => Progress = (float)x, start, end, 4, 1200, Easing.SinInOut);
        }

        public void AssociateEvent(TimeLineEvent eventControl, TransactionCompletion completion)
        {
            var percent = completion.ToPercentValue();

            if (!asociatedEvents.Contains(eventControl.Id))
                asociatedEvents.Add(eventControl.Id);

            if (!events.ContainsKey(eventControl.Id))
                events[eventControl.Id] = new List<Anchor>();

            events[eventControl.Id].Add(new Anchor(percent * info.Width, eventControl));

        //    anchors.Add(new Anchor(percent * info.Width, eventControl));
            InvalidateSurface();
        }

        public float GetCompletionPosition(TransactionCompletion completion)
        { 
            var percent = completion.ToPercentValue();
            return ((float)WidthRequest * percent) - TransactionLinkControl.ShapeRadius;
        }
    }
}
