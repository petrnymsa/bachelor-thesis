using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;


namespace BachelorThesis.Controls
{
    public enum TransactionLinkOrientation
    {
        Down = 0,
        Up = 1
    }

    public class TransactionLinkControl : SKCanvasView
    {
        private const int ShapeRadius = 8;
        private const int LineLength = 44;
        private const int ArrowLength = 8;
        private const int ArrowAngle = 35;

        #region Properties
        public static BindableProperty SourceTextProperty =
            BindableProperty.Create(nameof(SourceText), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public string SourceText
        {
            get => (string) GetValue(SourceTextProperty);
            set => SetValue(SourceTextProperty, value);
        }

        public static BindableProperty TargetTextProperty =
            BindableProperty.Create(nameof(TargetText), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public string TargetText
        {
            get => (string) GetValue(TargetTextProperty);
            set => SetValue(TargetTextProperty, value);
        }

        public static BindableProperty IsDashedProperty =
            BindableProperty.Create(nameof(IsDashed), typeof(bool), typeof(TransactionLinkControl), false,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public bool IsDashed
        {
            get => (bool)GetValue(IsDashedProperty);
            set => SetValue(IsDashedProperty, value);
        }

        public static BindableProperty LinkOrientationProperty =
            BindableProperty.Create(nameof(LinkOrientation), typeof(int), typeof(TransactionLinkControl), 0,
                BindingMode.TwoWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public int LinkOrientation
        {
            get => (int)GetValue(LinkOrientationProperty);
            set => SetValue(LinkOrientationProperty, value);
        }


        public static BindableProperty LinkStyleProperty =
            BindableProperty.Create(nameof(LinkStyle), typeof(int), typeof(TransactionLinkControl), 0,
                BindingMode.TwoWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        private readonly float arrowX;
        private readonly float arrowY;

        public int LinkStyle
        {
            get => (int)GetValue(LinkStyleProperty);
            set => SetValue(LinkStyleProperty, value);
        }

        #endregion
        public TransactionLinkControl()
        {
            this.HeightRequest = 100;
            arrowX = (float)(ArrowLength * Math.Sin(ArrowAngle * (Math.PI / 180)));
            arrowY = (float)(ArrowLength * Math.Cos(ArrowAngle * (Math.PI / 180)));
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Black.ToSKColor(),
                StrokeWidth = 1
            };
            

            var textPaint = new SKPaint
            {
                Color = Color.Black.ToSKColor(),
                TextSize = 10,
                IsAntialias = true
            };

            var dashedPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 1,
                StrokeCap = SKStrokeCap.Round,
                PathEffect = SKPathEffect.CreateDash(new float[] { 4, 3 }, 1)
            };

            canvas.Clear();

            var scale = (float)(e.Info.Width / this.Width); //scale canvas
            canvas.Scale(scale);

            if (LinkStyle == 0)
            {
                HeightRequest = 78;
                if (LinkOrientation == 0)
                    DrawDownSide(canvas, paint, textPaint, dashedPaint);
                else
                    DrawUpSide(canvas, paint, textPaint, dashedPaint);
            }
            else // State - Request
            {
                HeightRequest = 90;
                //circle
                canvas.Translate(ShapeRadius + 1, ShapeRadius + 1);
                canvas.DrawCircle(0, 0, ShapeRadius, paint);
                canvas.Translate(0, ShapeRadius);

                // source text
                canvas.Save();
                canvas.Translate(ShapeRadius, 5);
                canvas.DrawText(SourceText, 0, 0, textPaint);
                canvas.Restore();
                //line
                canvas.DrawLine(0, 0, 0, LineLength, IsDashed ? dashedPaint : paint);
                canvas.Translate(0, LineLength);
                canvas.DrawLine(0, 0, 0, ShapeRadius*3, IsDashed ? dashedPaint : paint);

                // line - arrow head
                canvas.Translate(0,ShapeRadius * 3);
                canvas.DrawLine(0,0,16,0, IsDashed ? dashedPaint : paint);
                // arrow
                canvas.Translate(16,0);
                canvas.DrawLine(0, 0, -arrowY, arrowX, paint);
                canvas.DrawLine(0, 0, -arrowY, -arrowX, paint);
            }

            paint.Dispose();
            textPaint.Dispose();
            dashedPaint.Dispose();
        }

        protected void DrawUpSide(SKCanvas canvas, SKPaint paint, SKPaint textPaint, SKPaint dashedPaint)
        {
            // square
            canvas.Translate(1, 1);
            canvas.DrawRect(new SKRect(0, 0, ShapeRadius * 2, ShapeRadius * 2), paint);
            canvas.Translate(ShapeRadius, ShapeRadius * 2);
            // target text
            canvas.Save();
            canvas.Translate(ShapeRadius / 2f + 5, ShapeRadius + 5);
            canvas.DrawText(TargetText, 0, 0, textPaint);
            canvas.Restore();

            //arrow
            DrawArrow(canvas, paint,false);

            // line
            canvas.DrawLine(0, 0, 0, LineLength, IsDashed ? dashedPaint : paint);

            canvas.Translate(0, LineLength);

            // source text
            canvas.Save();
            canvas.Translate(ShapeRadius, 0);
            canvas.DrawText(SourceText, 0, 0, textPaint);
            canvas.Restore();
            // circle
            canvas.Translate(0, ShapeRadius);
            canvas.DrawCircle(0, 0, ShapeRadius, paint);
        }

        protected void DrawArrow(SKCanvas canvas, SKPaint paint, bool downSide = true)
        {
          //  var arrowX = (float)(ArrowLength * Math.Sin(ArrowAngle * (Math.PI / 180)));
         //   var arrowY = (float)(ArrowLength * Math.Cos(ArrowAngle * (Math.PI / 180)));
            if (downSide)
            {
                canvas.DrawLine(0, 0, -arrowX, -arrowY, paint);
                canvas.DrawLine(0, 0, arrowX, -arrowY, paint);
            }
            else
            {
                canvas.DrawLine(0, 0, -arrowX, arrowY, paint);
                canvas.DrawLine(0, 0, arrowX, arrowY, paint);
            }
        }

        protected void DrawDownSide(SKCanvas canvas,SKPaint paint, SKPaint textPaint, SKPaint dashedPaint)
        {
            // circle
            canvas.Translate(ShapeRadius + 1, ShapeRadius + 1);
            canvas.DrawCircle(0, 0, ShapeRadius, paint);
            canvas.Translate(0, ShapeRadius);
            // source text
            canvas.Save();
            canvas.Translate(ShapeRadius, 5);
            canvas.DrawText(SourceText, 0, 0, textPaint);
            canvas.Restore();
            //line
            canvas.DrawLine(0, 0, 0, LineLength, IsDashed ? dashedPaint : paint);
            //arrow
            canvas.Translate(0, LineLength);
            DrawArrow(canvas, paint);
            // target text
            canvas.Save();
            canvas.Translate(ShapeRadius, -5);
            canvas.DrawText(TargetText, 0, 0, textPaint);
            canvas.Restore();
            // square
            canvas.Translate(-ShapeRadius, 0);
            canvas.DrawRect(new SKRect(0, 0, ShapeRadius * 2, ShapeRadius * 2), paint);
        }
    }
}