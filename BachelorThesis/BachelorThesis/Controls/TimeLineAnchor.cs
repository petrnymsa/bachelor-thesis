﻿using System;
using System.Collections.Generic;
using System.Text;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class TimeLineAnchor : ContentView
    {
//        public float BaseOffsetX { get; set; }
        public float OffsetX { get; set; }

        public bool IsRevealed { get; set; }

        public TransactionCompletion Completion { get; set; }
        public TimeLineEvent Event { get; set; }

        public TimeLineAnchor(TimeLineEvent timeEvent)
        {
            Event = timeEvent;
//            BaseOffsetX = baseOffsetX;
//            OffsetX = totalOffsetX;
            IsRevealed = false;
        }

    //    public float GetXPositionWithoutOffsets() => OffsetX - BaseOffsetX;
    }
}
