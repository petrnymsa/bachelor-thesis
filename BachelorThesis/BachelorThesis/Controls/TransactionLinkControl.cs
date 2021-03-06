﻿using System;
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
        public const float TextSpacing = 5f;
        private const int ArrowAngle = 35;
        private const int ArrowLength = 8;

        public const int LinkOrientationDown = (int)TransactionLinkOrientation.Down;
        public const int LinkOrientationUp = (int)TransactionLinkOrientation.Up;

        public const int StyleStateToState = (int)TransactionLinkStyle.StateToState;
        public const int StyleStateToRequest = (int)TransactionLinkStyle.StateToRequest;

        private readonly float spaceLength;

        #region Properties
        public static BindableProperty SourceTextProperty =
            BindableProperty.Create(nameof(SourceText), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public string SourceText
        {
            get => (string)GetValue(SourceTextProperty);
            set => SetValue(SourceTextProperty, value);
        }

        public static BindableProperty TargetTextProperty =
            BindableProperty.Create(nameof(TargetText), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public string TargetText
        {
            get => (string)GetValue(TargetTextProperty);
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

        public TransactionLinkOrientation LinkOrientation { get; set; }

        public TransactionLinkStyle LinkStyle { get; set; }

        public static BindableProperty BendWidthProperty =
            BindableProperty.Create(nameof(BendWidth), typeof(float), typeof(TransactionLinkControl), 0f,
                BindingMode.TwoWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public float BendWidth
        {
            get => (float)GetValue(BendWidthProperty);
            set => SetValue(BendWidthProperty, value);
        }

        public static BindableProperty SourceXProperty =
            BindableProperty.Create(nameof(SourceX), typeof(float), typeof(TransactionLinkControl), 0f,
                BindingMode.TwoWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public float SourceX
        {
            get => (float)GetValue(SourceXProperty);
            set => SetValue(SourceXProperty, value);
        }

        public static BindableProperty TargetXProperty =
            BindableProperty.Create(nameof(TargetX), typeof(float), typeof(TransactionLinkControl), 0f,
                BindingMode.TwoWay, propertyChanged:
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).InvalidateSurface(); });

        public float TargetX
        {
            get => (float)GetValue(TargetXProperty);
            set => SetValue(TargetXProperty, value);
        }

        public TransactionCompletion SourceCompletion { get; set; }

        public TransactionCompletion TargetCompletion { get; set; }

        public TransactionBoxControl SourceBox { get; set; }

        public TransactionBoxControl TargetBox { get; set; }

        public TransactionCompletion OffsetCompletion { get; set; }


        private bool isReflected = false;

        public bool IsReflected
        {
            get => isReflected;
            set { isReflected = value; InvalidateSurface(); }
        }

        private readonly float arrowX;
        private readonly float arrowY;
        private Color strokeColor;


        #endregion

        public TransactionLinkControl(TransactionCompletion sourceCompletion, TransactionCompletion targetCompletion,
            TransactionBoxControl sourceBox, TransactionBoxControl targetBox, TransactionLinkOrientation orientation = TransactionLinkOrientation.Down,
            TransactionLinkStyle style = TransactionLinkStyle.StateToState, TransactionCompletion offsetCompletion = TransactionCompletion.None,
            float space = 60f)
        {
            SourceCompletion = sourceCompletion;
            TargetCompletion = targetCompletion;
            SourceBox = sourceBox;
            TargetBox = targetBox;
            LinkOrientation = orientation;
            LinkStyle = style;
            OffsetCompletion = offsetCompletion;
            IsDashed = LinkOrientation == TransactionLinkOrientation.Up;

            SourceX = sourceBox.GetCompletionPosition(sourceCompletion);
            TargetX = targetBox.GetCompletionPosition(targetCompletion) - targetBox.GetCompletionOffset(OffsetCompletion);

            //            SourceText = SourceCompletion.AsAbbreviation();
            //            TargetText = TargetCompletion.AsAbbreviation();
            SourceText = SourceCompletion.ToString();
            TargetText = TargetCompletion.ToString();

            arrowX = (float)(ArrowLength * Math.Sin(ArrowAngle * (Math.PI / 180)));
            arrowY = (float)(ArrowLength * Math.Cos(ArrowAngle * (Math.PI / 180)));

            spaceLength = space - ShapeRadius * 2;
            strokeColor = Color.Black;

            RefreshLayout();

            // react to box sizes
            sourceBox.SizeChanged += (sender, args) => RefreshLayout();
            targetBox.SizeChanged += (sender, args) => RefreshLayout();
            //set as source link
            SourceBox.AddLink(this);

           // BackgroundColor = Color.LightGray;
        }

        public void RefreshLayout()
        {
            // little hack
            if (TargetBox == null || SourceBox == null)
                return;

            float leftSpace = 0;
            float offsetX = 0;

            SourceX = SourceBox.GetCompletionPosition(SourceCompletion);
            TargetX = TargetBox.GetCompletionPosition(TargetCompletion) - TargetBox.GetCompletionOffset(OffsetCompletion);
            IsReflected = SourceX <= TargetX;

            if (LinkStyle == TransactionLinkStyle.StateToState)
                offsetX = SourceX <= TargetX ? SourceX : TargetX;
            else offsetX = SourceX;

            if (LinkOrientation == TransactionLinkOrientation.Down)
                leftSpace = SourceBox.GetCompletionOffset(OffsetCompletion);
            else leftSpace = TargetBox.GetCompletionOffset(OffsetCompletion);

            if (LinkStyle == TransactionLinkStyle.StateToState)
                RelativeLayout.SetXConstraint(this, Constraint.RelativeToView(TargetBox, (parent, sibling) => sibling.X + leftSpace + offsetX - ShapeRadius));
            else
                RelativeLayout.SetXConstraint(this, Constraint.RelativeToView(SourceBox, (parent, sibling) => sibling.X + leftSpace + offsetX - ShapeRadius * 2 - StateToRequestArrowHead));

            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = strokeColor.ToSKColor(),
                StrokeWidth = 1
            };

            var textPaint = new SKPaint
            {
                Color = strokeColor.ToSKColor(),
                TextSize = 10,
                IsAntialias = true
            };

            var dashedPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = strokeColor.ToSKColor(),
                StrokeWidth = 1,
                StrokeCap = SKStrokeCap.Round,
                PathEffect = SKPathEffect.CreateDash(new float[] { 4, 3 }, 1)
            };

            canvas.Clear();
            HeightRequest = 4 * ShapeRadius + spaceLength + paint.StrokeWidth*2;
            WidthRequest = 3 * ShapeRadius + textPaint.MeasureText(SourceText.Length >= TargetText.Length ? SourceText : TargetText);

            var scale = (float)(e.Info.Width / this.Width); //scale canvas
            canvas.Scale(scale);



            if (LinkStyle == StyleStateToState)
            {
                WidthRequest = Math.Abs(SourceX - TargetX) + ShapeRadius*2 + textPaint.MeasureText(SourceText.Length >= TargetText.Length ? SourceText : TargetText) + TextSpacing;

                if (LinkOrientation == LinkOrientationDown)
                    DrawBendedDownSide(canvas, paint, textPaint, dashedPaint);
                else
                    DrawBendedUpSide(canvas, paint, textPaint, dashedPaint);
            }
            else // State - Request
            {
                HeightRequest += ShapeRadius * 3 + 1 + arrowY;
                DrawStateToRequestDownSide(canvas, paint, textPaint, dashedPaint);
            }


            paint.Dispose();
            textPaint.Dispose();
            dashedPaint.Dispose();
        }

        private void DrawBendedDownSide(SKCanvas canvas, SKPaint paint, SKPaint textPaint, SKPaint dashedPaint)
        {
            //circle
            canvas.Translate(ShapeRadius + paint.StrokeWidth, ShapeRadius + paint.StrokeWidth);
            canvas.DrawCircle(0, 0, ShapeRadius, paint);
            canvas.Translate(0, ShapeRadius);

            // source text
            canvas.Save();
            canvas.Translate(ShapeRadius, TextSpacing);
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
            canvas.Translate(ShapeRadius, -2* TextSpacing);
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
            if (IsReflected)
                canvas.Translate(Math.Abs(SourceX - TargetX), 0);
            canvas.DrawRect(new SKRect(0, 0, ShapeRadius * 2, ShapeRadius * 2), paint);
            canvas.Translate(ShapeRadius, ShapeRadius * 2);
            // target text
            canvas.Save();
            canvas.Translate(ShapeRadius / 2f + TextSpacing, ShapeRadius + TextSpacing);
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
            canvas.Translate(ShapeRadius, -2 * TextSpacing);
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
            var width = SourceX - TargetX;
            canvas.DrawLine(0, 0, width, 0, IsDashed ? dashedPaint : paint);
            canvas.Translate(width, 0);
            // line
            canvas.DrawLine(0, 0, 0, spaceLength / 2, IsDashed ? dashedPaint : paint);
            canvas.Translate(0, spaceLength / 2);
        }

        private void DrawStateToRequestDownSide(SKCanvas canvas, SKPaint paint, SKPaint textPaint, SKPaint dashedPaint)
        {
            //circle
            canvas.Translate(ShapeRadius + paint.StrokeWidth, ShapeRadius + paint.StrokeWidth);
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

            //line
            canvas.DrawLine(0, 0, 0, spaceLength + ShapeRadius * 2, IsDashed ? dashedPaint : paint);
            canvas.Translate(0, spaceLength + ShapeRadius * 2);
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