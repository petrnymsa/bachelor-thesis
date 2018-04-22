using System;
using System.Collections.Generic;
using System.Text;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class TimeLineAnchor : ContentView
    {
        public float BaseOffsetX { get; set; }
        public float TotalOffsetX { get; set; }

        public bool IsRevealed { get; set; }

        public TransactionCompletion Completion { get; set; }


        public TimeLineAnchor(float baseOffsetX, float totalOffsetX)
        {
            BaseOffsetX = baseOffsetX;
            TotalOffsetX = totalOffsetX;
            IsRevealed = false;
        }

        public float GetXPositionWithoutOffsets() => TotalOffsetX - BaseOffsetX;
    }
}
