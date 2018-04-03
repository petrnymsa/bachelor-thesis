using System;
using NControl.Abstractions;
using NGraphics;
using Xamarin.Forms;
using Font = NGraphics.Font;
using Point = NGraphics.Point;

namespace BachelorThesis.Controls
{
    public class TransactionLinkControl : NControlView
    {
        #region Properties
        public static BindableProperty SourceTextProperty =
            BindableProperty.Create(nameof(SourceText), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).Invalidate(); });

        public string SourceText
        {
            get => (string) GetValue(SourceTextProperty);
            set => SetValue(SourceTextProperty, value);
        }

        public static BindableProperty TargetTextProperty =
            BindableProperty.Create(nameof(TargetText), typeof(string), typeof(TransactionLinkControl), string.Empty,
                BindingMode.OneWay, null,
                (bindable, oldValue, newValue) => { (bindable as TransactionLinkControl).Invalidate(); });

        public string TargetText
        {
            get => (string) GetValue(TargetTextProperty);
            set => SetValue(TargetTextProperty, value);
        }
#endregion

        protected void DrawDashedLine(ICanvas canvas, int totalLength)
        {
            var piecesCount = 10;
            var pieceLength = totalLength / piecesCount;
            var gapLength = pieceLength / 2;

            var length = 0;
            for (int i = 0; i < piecesCount && length < totalLength; i++)
            {
                var translateY = pieceLength + gapLength;
                canvas.DrawLine(0, 0, 0, pieceLength, Colors.Red, 4);
                canvas.Translate(0, translateY);

                length += translateY;
            }
        }

        protected void DrawText(ICanvas canvas, string text)
        {
            canvas.DrawText(text, new Point(0, 0), new Font {Size = 20}, Colors.Red);
        }

        public override void Draw(NGraphics.ICanvas canvas, NGraphics.Rect rect)
        {
            
            var shapeWidth = 40;
            var pen = new Pen(Colors.Black, 4);

            var lineLength = 140;
            var arrowLength = 25;
            var arrowAngle = 35;
            var arrowX = arrowLength * Math.Sin(arrowAngle * (Math.PI / 180));
            var arrowY = arrowLength * Math.Cos(arrowAngle * (Math.PI / 180));

            canvas.DrawEllipse(0, 0, shapeWidth, shapeWidth, pen);
            // draw source act
            canvas.SaveState();
            canvas.Translate(shapeWidth + 10, shapeWidth + 20);
            DrawText(canvas, SourceText);
            canvas.RestoreState();
            //  draw line
            canvas.Translate(shapeWidth / 2d, shapeWidth);

            canvas.SaveState();
            DrawDashedLine(canvas, lineLength);
            canvas.RestoreState();
            // draw arrow
            canvas.Translate(0, lineLength);


            canvas.FillPath(path =>
            {
                path.LineTo(-arrowX, -arrowY);
                path.LineTo(arrowX, -arrowY);
                path.MoveTo(arrowX, -arrowY);
                path.LineTo(arrowX * 2, 0);
                path.Close();
            }, Brushes.Black);
            canvas.SaveState();
            canvas.Translate(shapeWidth / 2d + 10, 0);
            DrawText(canvas, TargetText);

            canvas.RestoreState();
            canvas.Translate(-shapeWidth / 2d, 0);
            //draw destination
            canvas.DrawRectangle(0, 0, shapeWidth, shapeWidth, pen);

        }
    }
}