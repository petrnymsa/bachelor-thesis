using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class TimeLineAnchor : ContentView
    {
        public double LeftX { get; set; }

        public double RightX => LeftX + Width;

        public double MiddleX => LeftX + Width * 0.5;

        public TimeLineAnchor(double leftX)
        {
            LeftX = leftX;
        }

        //        public bool IsRevealed { get; set; }
    }
}
