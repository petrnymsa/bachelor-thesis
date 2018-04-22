using System;
using System.Collections.Generic;
using System.Text;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class TimeLineEventAnchor : TimeLineAnchor
    {
        public TransactionCompletion Completion { get; set; }
        public TimeLineEvent Event { get; set; }

        public TimeLineEventAnchor(TimeLineEvent timeEvent)
        {
            Event = timeEvent;
        }
    }

    public class TimeLineAnchor : ContentView
    {
        public float OffsetX { get; set; }

//        public bool IsRevealed { get; set; }
    }
}
