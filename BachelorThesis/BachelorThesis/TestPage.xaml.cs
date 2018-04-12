using System;
using System.Collections.Generic;
using System.Diagnostics;
using BachelorThesis.Business.DataModels;
using BachelorThesis.Controls;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;

namespace BachelorThesis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {

        class Anchor
        {
            public SKPoint point;
            public bool isFocused;

            public Anchor(float x, float y, bool isFocused = false)
            {
                point = new SKPoint(x, y);
                this.isFocused = isFocused;
            }

            public bool HitTestX(SKPoint p, float tolerance)
            {
                return Math.Abs(point.X - p.X) <= tolerance;
            }
        }
        List<Anchor> anchors = new List<Anchor>();

        List<SKPoint> touches = new List<SKPoint>();
        private SKPoint lastTouch;

        private bool isPressed = false;

        public TestPage()
        {
            InitializeComponent();


        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            // CLEARING THE SURFACE

            // we get the current surface from the event args
            var surface = e.Surface;
            // then we get the canvas that we can draw on
            var canvas = surface.Canvas;
            // clear the canvas / view
            canvas.Clear(Color.Aquamarine.ToSKColor());

            if (anchors.Count == 0)
            {
                var percent = 0.2f;
                for (int i = 0; i < 5; i++)
                {
                    anchors.Add(new Anchor(e.Info.Width * percent, e.Info.Height / 2f));
                    percent += 0.15f;
                }

            }


            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Fill;
                paintTouchPoint.Color = SKColors.Red;

                foreach (var anchor in anchors)
                {
                    if (anchor.isFocused)
                        canvas.DrawCircle(anchor.point.X, anchor.point.Y, 8, paintTouchPoint);
                    else
                        canvas.DrawCircle(anchor.point.X, anchor.point.Y, 4, paintTouchPoint);

                }

                if (isPressed)
                {
                    paintTouchPoint.Color = SKColors.Black;
                    canvas.DrawLine(lastTouch.X, 0, lastTouch.X, e.Info.Height, paintTouchPoint);
                }
            }

        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {

            if (e.ActionType ==
                SkiaSharp.Views.Forms.SKTouchAction.Pressed)
            {
                lastTouch = e.Location;
                isPressed = true;
           //     touches.Add(lastTouch);
            }
            else if(e.ActionType == SKTouchAction.Moved)
                    lastTouch = e.Location;
            else if (e.ActionType == SKTouchAction.Released)
            {
                isPressed = false;
            }



            e.Handled = true;
            if (isPressed)
            {
                foreach (var anchor in anchors)
                {
                    if (anchor.HitTestX(lastTouch, 20))
                        anchor.isFocused = true;
                    else anchor.isFocused = false;
                }
            }
            else anchors.ForEach(x=> x.isFocused = false);

            // update the Canvas as you wish
            ((SKCanvasView)sender).InvalidateSurface();
        }

        private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
        {
            
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var id = entryHour.Text;
            //var act = entryMinute.Text;
            var act = new Random().Next(0, 6);
            timeline.AddEvent(id,(TransactionCompletion)act, Color.Red);
        }
    }
}