using System;
using System.Collections.Generic;
using SkiaSharp;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();

            columnSeries.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Jan", 40.5),
                new ChartDataPoint("Feb", 37.1),
                new ChartDataPoint("Mar", 43.4),
                new ChartDataPoint("Apr", 41.8),
            };

            pieSeries.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Accepted", 67),
                new ChartDataPoint("Stated", 33),
            };



            lineSeries.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Mon", 32),
                new ChartDataPoint("Tue", 31),
                new ChartDataPoint("Wed", 29),
                new ChartDataPoint("Thu", 34),
                new ChartDataPoint("Fri", 33),
                new ChartDataPoint("Sat", 45),
                new ChartDataPoint("Sun", 11),
            };
        }

    }


}