using System;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
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

    public enum TransactionLinkStyle
    {
        StateToState = 0,
        StateToRequest = 1
    }

    public class TransactionLinkControl : SKCanvasView
    {
        public const float ShapeRadius = 4;
        public const float StateToRequestArrowHead = 16;

        public const int LinkOrientationDown = (int)TransactionLinkOrientation.Down;
        public const int LinkOrientationUp = (int)TransactionLinkOrientation.Up;

        public const int StyleStateToState = (int)TransactionLinkStyle.StateToState;
        public const int StyleStateToRequest = (int)TransactionLinkStyle.StateToRequest;

        private readonly float spaceLength;
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

        public static BindableProperty SourceCardinalityProperty =
            BindableProperty.Create(nameof(SourceCardinality), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public string SourceCardinality
        {
            get => (string)GetValue(SourceCardinalityProperty);
            set => SetValue(SourceCardinalityProperty, value);
        }

        public static BindableProperty TargetCardinalityProperty =
            BindableProperty.Create(nameof(TargetCardinality), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public string TargetCardinality
        {
            get => (string)GetValue(TargetCardinalityProperty);
            set => SetValue(TargetCardinalityProperty, value);
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

        public int LinkStyle
        {
            get => (int)GetValue(LinkStyleProperty);
            set => SetValue(LinkStyleProperty, value);
        }

        public static BindableProperty BendWidthProperty =
            BindableProperty.Create(nameof(BendWidth), typeof(float), typeof(TransactionLinkControl), 0f,
                BindingMode.TwoWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public float BendWidth
        {
            get => (float)GetValue(BendWidthProperty);
            set => SetValue(BendWidthProperty, value);
        }

        private readonly float arrowX;
        private readonly float arrowY;

        #endregion
        public TransactionLinkControl(float space = 60f)
        {
            arrowX = (float)(ArrowLength * Math.Sin(ArrowAngle * (Math.PI / 180)));
            arrowY = (float)(ArrowLength * Math.Cos(ArrowAngle * (Math.PI / 180)));

            spaceLength = space - ShapeRadius*2;
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
            HeightRequest = 4 * ShapeRadius + spaceLength + paint.StrokeWidth;
            WidthRequest = 3 * ShapeRadius + paint.MeasureText(SourceText); 

            var scale = (float)(e.Info.Width / this.Width); //scale canvas
            canvas.Scale(scale);

            if (LinkStyle == StyleStateToState)
            {
                WidthRequest = BendWidth + ShapeRadius * 5 + 5;

                if (LinkOrientation == 0)
                    DrawBendedDownSide(canvas, paint, textPaint, dashedPaint);
                else
                    DrawBendedUpSide(canvas, paint, textPaint, dashedPaint);
            }
            else // State - Request
            {
                HeightRequest += ShapeRadius + 1 + arrowY;
                DrawStateToRequestDownSide(canvas, paint, textPaint, dashedPaint);
            }


            paint.Dispose();
            textPaint.Dispose();
            dashedPaint.Dispose();
        }

        private void DrawBendedDownSide(SKCanvas canvas, SKPaint paint, SKPaint textPaint, SKPaint dashedPaint)
        {
            //circle
            canvas.Translate(ShapeRadius + 1, ShapeRadius + 1);
            canvas.DrawCircle(0, 0, ShapeRadius, paint);
            canvas.Translate(0, ShapeRadius);

            // source text
            canvas.Save();
            canvas.Translate(ShapeRadius, 5);
            canvas.DrawText(SourceText, 0, 0, textPaint);

            if (!string.IsNullOrEmpty(SourceCardinality))
            {
                canvas.Translate(0, textPaint.FontSpacing);
                canvas.DrawText(SourceCardinality, 0, 0, textPaint);
            }

            canvas.Restore();
            // line
            DrawBendedLine(canvas, paint, dashedPaint);

            //arrow
            DrawArrow(canvas, paint);
            // target text
            canvas.Save();
            canvas.Translate(ShapeRadius, -10);
            canvas.DrawText(TargetText, 0, 0, textPaint);

            if (!string.IsNullOrEmpty(TargetCardinality))
            {
                canvas.Translate(0, textPaint.FontSpacing);
                canvas.DrawText(TargetCardinality, 0, 0, textPaint);
            }
            canvas.Restore();

            // square
            canvas.Translate(-ShapeRadius, 0);
            canvas.DrawRect(new SKRect(0, 0, ShapeRadius * 2, ShapeRadius * 2), paint);
        }

        private void DrawBendedUpSide(SKCanvas canvas, SKPaint paint, SKPaint textPaint, SKPaint dashedPaint)
        {
            canvas.DrawRect(new SKRect(0, 0, ShapeRadius * 2, ShapeRadius * 2), paint);
            canvas.Translate(ShapeRadius, ShapeRadius * 2);
            // target text
            canvas.Save();
            canvas.Translate(ShapeRadius / 2f + 5, ShapeRadius + 5);
            canvas.DrawText(TargetText, 0, 0, textPaint);
            if (!string.IsNullOrEmpty(TargetCardinality))
            {
                canvas.Translate(0, textPaint.FontSpacing);
                canvas.DrawText(TargetCardinality, 0, 0, textPaint);
            }
            canvas.Restore();

            //arrow
            DrawArrow(canvas, paint, false);
             // line
            DrawBendedLine(canvas, paint, dashedPaint);

            // source text
            canvas.Save();
            canvas.Translate(ShapeRadius, -10);
            canvas.DrawText(SourceText, 0, 0, textPaint);
            if (!string.IsNullOrEmpty(SourceCardinality))
            {
                canvas.Translate(0, textPaint.FontSpacing);
                canvas.DrawText(SourceCardinality, 0, 0, textPaint);
            }
            canvas.Restore();
            // circle
            canvas.Translate(0, ShapeRadius);
            canvas.DrawCircle(0, 0, ShapeRadius, paint);
        }

        private void DrawBendedLine(SKCanvas canvas, SKPaint paint, SKPaint dashedPaint)
        {
            // line
            canvas.DrawLine(0, 0, 0, spaceLength / 2, IsDashed ? dashedPaint : paint);
            canvas.Translate(0, spaceLength / 2);
            // bend
            canvas.DrawLine(0, 0, BendWidth, 0, IsDashed ? dashedPaint : paint);
            canvas.Translate(BendWidth, 0);
            // line
            canvas.DrawLine(0, 0, 0, spaceLength / 2, IsDashed ? dashedPaint : paint);
            canvas.Translate(0, spaceLength / 2);
        }

        private void DrawStateToRequestDownSide(SKCanvas canvas, SKPaint paint, SKPaint textPaint, SKPaint dashedPaint)
        {
            //circle
            canvas.Translate(ShapeRadius + 1, ShapeRadius + 1);
            canvas.DrawCircle(0, 0, ShapeRadius, paint);
            canvas.Translate(0, ShapeRadius);

            // source text
            canvas.Save();
            canvas.Translate(ShapeRadius, 5);
            canvas.DrawText(SourceText, 0, 0, textPaint);

            if (!string.IsNullOrEmpty(SourceCardinality))
            {
                canvas.Translate(0, textPaint.FontSpacing);
                canvas.DrawText(SourceCardinality,0,0,textPaint);
            }

            canvas.Restore();

            

            //line
            canvas.DrawLine(0, 0, 0, spaceLength, IsDashed ? dashedPaint : paint);
            canvas.Translate(0, spaceLength);
            canvas.DrawLine(0, 0, 0, ShapeRadius * 3, IsDashed ? dashedPaint : paint);

            // line - arrow head
            canvas.Translate(0, ShapeRadius * 3);
            canvas.DrawLine(0, 0, StateToRequestArrowHead, 0, IsDashed ? dashedPaint : paint);
            // arrow
            canvas.Translate(StateToRequestArrowHead, 0);
            canvas.DrawLine(0, 0, -arrowY, arrowX, paint);
            canvas.DrawLine(0, 0, -arrowY, -arrowX, paint);
        }

        protected void DrawArrow(SKCanvas canvas, SKPaint paint, bool downSide = true)
        {
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
    }
}