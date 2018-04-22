using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HourMinuteAnchor : TimeLineAnchor
    {

        public static readonly BindableProperty HourProperty = BindableProperty.Create(nameof(Hour), typeof(int), typeof(HourMinuteAnchor), 0);

        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set { SetValue(HourProperty, value); OnPropertyChanged(nameof(FormattedHourMinute)); }
        }


        public static readonly BindableProperty MinuteProperty = BindableProperty.Create(nameof(Minute), typeof(int), typeof(HourMinuteAnchor), 0);

        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set { SetValue(MinuteProperty, value); OnPropertyChanged(nameof(FormattedHourMinute)); }
        }

        public string FormattedHourMinute => $"{Hour:00}:{Minute:00}";

        //public ObservableCollection<TimeLineEvent> Events { get; set; }

     //   public TimeLineEvent Event { get; set; }

        public HourMinuteAnchor()
        :base(null)
        {
            InitializeComponent();
            // Events = new ObservableCollection<TimeLineEvent>();

            this.Content.BindingContext = this;
        }

        public HourMinuteAnchor(int hour, int minute, TimeLineEvent timeEvent)
        :base(timeEvent)
        {
            InitializeComponent();
           // Events = new ObservableCollection<TimeLineEvent>();

            this.Content.BindingContext = this;
            Hour = hour;
            Minute = minute;
        }

        //public TimeLineEvent AddEvent(string identifier, TransactionCompletion completion, Color color)
        //{
        //    var existing = Events.FirstOrDefault(x => x.TransactionIdentifier == identifier);

        //    if (existing != null)
        //    {
        //        existing.AddAct(completion.AsAbbreviation());
        //        return existing;
        //    }

        //    var eventControl = new TimeLineEvent(identifier, completion.AsAbbreviation(), color);
        //    Events.Add(eventControl);

        //    return eventControl;
        //}

        //private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    ((ListView)sender).SelectedItem = null;
        //}
    }
}