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


        public static readonly BindableProperty WithEventProperty = BindableProperty.Create(nameof(WithEvent), typeof(bool), typeof(TimeLineEventAnchor), true);

        public bool WithEvent
        {
            get => (bool)GetValue(WithEventProperty);
            set => SetValue(WithEventProperty, value);
        }

        public TimeLineEventAnchor(double leftX, TimeLineEvent timeEvent, TransactionCompletion completion) : base(leftX)
        {
            Event = timeEvent;
            Completion = completion;
        }
    }

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
