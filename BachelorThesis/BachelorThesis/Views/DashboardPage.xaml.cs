using System.Collections.Generic;
using Microcharts;
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

            chart.Series.Add(new ColumnSeries()
            {
                ItemsSource = new List<ChartDataPoint>
                {
                    new ChartDataPoint("Jan", 150),
                    new ChartDataPoint("Feb", 100),
                    new ChartDataPoint("Mar", 108),
                    new ChartDataPoint("Apr", 180),
                },
                
            });
        }
    }


}