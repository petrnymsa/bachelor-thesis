﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using PropertyChanged;
using Xamarin.Forms;

namespace BachelorThesis.Controls
{
    public class TimeLineEvent : BindableBase
    {
        private static int nextId = 0;
        //public event PropertyChangedEventHandler PropertyChanged;
        public int Id { get; set; }
        [AlsoNotifyFor(nameof(FormattedString))]
        public string TransactionIdentifier { get; set; }
        //[AlsoNotifyFor(nameof(FormattedString))]
        //public string CAct { get; set; }
        public Color Color { get; set; }
        public DateTime Created { get; set; }
        public string FormattedString => $"{TransactionIdentifier}[{String.Join(",", acts)}]";

        public bool IsRevealed { get; set; }

        public TransactionEvent Event { get; set; }

        private List<string> acts;

        public TimeLineEvent(string transactionIdentifier,TransactionEvent transactionEvent, Color color)
        {
            TransactionIdentifier = transactionIdentifier;
            Color = color;
            Created = transactionEvent.Created;
            acts = new List<string>() {transactionEvent.Completion.AsAbbreviation()};
            Id = nextId++;
            IsRevealed = false;
            Event = transactionEvent;
        }

        public void AddAct(string act)
        {
            acts.Add(act);
        //    acts.Sort(StringComparer.InvariantCulture);
            OnPropertyChanged(nameof(FormattedString));
        }

    }
}
