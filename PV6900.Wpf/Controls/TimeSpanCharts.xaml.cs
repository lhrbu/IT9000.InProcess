using PV6900.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PV6900.Wpf.Controls
{
    public partial class TimeSpanCharts : Grid
    {
        public TimeSpanCharts()
        {
            InitializeComponent();
        }

        public object TimeSpanVoltaChartDataContext
        {
            get => (object)GetValue(TimeSpanVoltaChartDataContextProperty);
            set => SetValue(TimeSpanVoltaChartDataContextProperty, value);
        }
        public static readonly DependencyProperty TimeSpanVoltaChartDataContextProperty =
            DependencyProperty.Register(nameof(TimeSpanVoltaChartDataContext), typeof(object), typeof(TimeSpanCharts));

        public object TimeSpanAmpereChartDataContext
        {
            get => (object)GetValue(TimeSpanAmpereChartDataContextProperty);
            set => SetValue(TimeSpanAmpereChartDataContextProperty, value);
        }
        public static readonly DependencyProperty TimeSpanAmpereChartDataContextProperty =
            DependencyProperty.Register(nameof(TimeSpanAmpereChartDataContext), typeof(object), typeof(TimeSpanCharts));
    }
}