using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    //public class TimeLineItem
    //{
    //    public int Hour { get; set; }
    //    public int Minute { get; set; }

    //    public ObservableCollection<TimeLineEvent> Events { get; set; }
    //}

    public class TimeLineEvent
    {
        public string TransactionIdentifier { get; set; }
        public string CAct { get; set; }
        public Color Color { get; set; }
        public string FormattedString => $"{TransactionIdentifier}[{CAct}]";

        public TimeLineEvent(string transactionIdentifier, string cAct, Color color)
        {
            TransactionIdentifier = transactionIdentifier;
            CAct = cAct;
            Color = color;
        }

        public override string ToString()
        {
            return $"{TransactionIdentifier}[{CAct}]";
        }
    }
}
