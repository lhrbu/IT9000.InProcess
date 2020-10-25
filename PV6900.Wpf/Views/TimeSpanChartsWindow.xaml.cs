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

namespace PV6900.Wpf.Views
{
    /// <summary>
    /// TimeSpanChartsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimeSpanChartsWindow : Window
    {
        public TimeSpanChartsWindow(
            TimeSpanVoltaChartVM timeSpanVoltaChartVM,
            TimeSpanAmpereChartVM timeSpanAmpereChartVM)
        {
            InitializeComponent();
            LineSeries_TimeSpanVoltaChart.DataContext = timeSpanVoltaChartVM;
            LineSeries_TimeSpanAmpereChart.DataContext = timeSpanAmpereChartVM;
        }
    }
}
