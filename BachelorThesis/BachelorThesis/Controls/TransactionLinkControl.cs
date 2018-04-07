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


        #endregion


        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            var shapeRadius = 8;
            var lineLength = 44;
            var arrowLength = 8;
            var arrowAngle = 35;
            var arrowX = (float)(arrowLength * Math.Sin(arrowAngle * (Math.PI / 180)));
            var arrowY = (float)(arrowLength * Math.Cos(arrowAngle * (Math.PI / 180)));

            

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

            canvas.Clear();

            var scale = (float)(e.Info.Width / this.Width); //scale canvas
            canvas.Scale(scale);

            if (LinkOrientation == 0)
            {
                // circle
                canvas.Translate(shapeRadius + 1, shapeRadius + 1);
                canvas.DrawCircle(0, 0, shapeRadius, paint);
                canvas.Translate(0, shapeRadius);
                // source text
                canvas.Save();
                canvas.Translate(shapeRadius, 5);
                canvas.DrawText(SourceText, 0, 0, textPaint);
                canvas.Restore();
                // line
                if (IsDashed)
                    DrawDashedLine(canvas, lineLength);
                else
                    canvas.DrawLine(0, 0, 0, lineLength, paint);
                //arrow
                canvas.Translate(0, lineLength);
                canvas.DrawLine(0, 0, -arrowX, -arrowY, paint);
                canvas.DrawLine(0, 0, arrowX, -arrowY, paint);
                // target text
                canvas.Save();
                canvas.Translate(shapeRadius, -5);
                canvas.DrawText(TargetText, 0, 0, textPaint);
                canvas.Restore();
                // square
                canvas.Translate(-shapeRadius, 0);
                canvas.DrawRect(new SKRect(0, 0, shapeRadius * 2, shapeRadius * 2), paint);
            }
            else
            {
                // square
                canvas.Translate(1, 1);
                canvas.DrawRect(new SKRect(0, 0, shapeRadius * 2, shapeRadius * 2), paint);
                canvas.Translate(shapeRadius, shapeRadius * 2);
                // target text
                canvas.Save();
                canvas.Translate(shapeRadius / 2f + 5, shapeRadius + 5);
                canvas.DrawText(TargetText, 0, 0, textPaint);
                canvas.Restore();

                //arrow
                canvas.DrawLine(0, 0, -arrowX, arrowY, paint);
                canvas.DrawLine(0, 0, arrowX, arrowY, paint);

                // line
                if (IsDashed)
                    DrawDashedLine(canvas, lineLength);
                else
                    canvas.DrawLine(0, 0, 0, lineLength, paint);

                canvas.Translate(0,lineLength);

                // source text
                canvas.Save();
                canvas.Translate(shapeRadius, 0);
                canvas.DrawText(SourceText, 0, 0, textPaint);
                canvas.Restore();
                // circle
                canvas.Translate(0, shapeRadius);
                canvas.DrawCircle(0, 0, shapeRadius, paint);

            }



        }

        private static void DrawSquare(SKCanvas canvas, int shapeRadius, SKPaint paint)
        {
          
        }

        private void DrawTargetText(SKCanvas canvas, int shapeRadius, SKPaint textPaint)
        {
         
        }

        private void DrawSourceText(SKCanvas canvas, int shapeRadius, SKPaint textPaint)
        {
           
        }

        private static void DrawCircle(SKCanvas canvas, int shapeRadius, SKPaint paint)
        {
           
        }

        protected void DrawDashedLine(SKCanvas canvas, int totalLength)
        {
            var piecesCount = 10;
            var pieceLength = totalLength / piecesCount;
            var gapLength = pieceLength / 2;

            var length = 0;

            canvas.Save();
            for (var i = 0; i < piecesCount && length < totalLength; i++)
            {
                var translateY = pieceLength + gapLength;
               
                canvas.DrawLine(0, 0, 0, pieceLength,new SKPaint()
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.Black.ToSKColor(),
                    StrokeWidth = 1
                });

                canvas.Translate(0, translateY);

                length += translateY;
            }

            canvas.Restore();
        }
    }
}