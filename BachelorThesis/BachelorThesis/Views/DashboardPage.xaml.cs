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

            contractAverageWaiting.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Jan", 40.5),
                new ChartDataPoint("Feb", 37.1),
                new ChartDataPoint("Mar", 43.4),
                new ChartDataPoint("Apr", 41.8),
            };

            monthSuccessRate.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Accepted", 67),
                new ChartDataPoint("Failed", 33),
            };



            dailyNewContracts.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Mon", 16),
                new ChartDataPoint("Tue", 22),
                new ChartDataPoint("Wed", 15),
                new ChartDataPoint("Thu", 34),
                new ChartDataPoint("Fri", 48),
                new ChartDataPoint("Sat", 38),
                new ChartDataPoint("Sun", 25),
            };

            incomeSeries.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Jan", 5000),
                new ChartDataPoint("Feb", 4800),
                new ChartDataPoint("Mar", 6800),
                new ChartDataPoint("Apr", 6000),
            };

            expenseSeries.ItemsSource = new List<ChartDataPoint>
            {
                new ChartDataPoint("Jan", 3900),
                new ChartDataPoint("Feb", 4200),
                new ChartDataPoint("Mar", 4500),
                new ChartDataPoint("Apr", 5200),
            };
        }

    }


}