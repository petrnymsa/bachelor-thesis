using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BachelorThesis.Business.DataModels;
using PropertyChanged;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class TimeLineEvent : BindableBase
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        [AlsoNotifyFor(nameof(FormattedString))]
        public string TransactionIdentifier { get; set; }
        //[AlsoNotifyFor(nameof(FormattedString))]
        //public string CAct { get; set; }
        public Color Color { get; set; }
        public string FormattedString => $"{TransactionIdentifier}[{String.Join(",", acts)}]";

        private List<string> acts;

        public TimeLineEvent(string transactionIdentifier, string cAct, Color color)
        {
            TransactionIdentifier = transactionIdentifier;
            Color = color;

            acts = new List<string>() {cAct};
         
        }

        public void AddAct(string act)
        {
            acts.Add(act);
            OnPropertyChanged(nameof(FormattedString));
        }

    }
}
