using IT9000.Wpf.Shared.Models;
using Prism.Mvvm;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Services;
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

namespace PV6900.Wpf
{
    /// <summary>
    /// PV6900Window.xaml 的交互逻辑
    /// </summary>
    public partial class PV6900Window : Grid
    {
        public PV6900Window(
            PV6900WindowVM pv6900WindowVM,
            DeviceMonitorService deviceMonitorService
            )
        {
            InitializeComponent();
            DataContext = pv6900WindowVM;
            deviceMonitorService.BeforeFetchPoint += (sender, e) => Chart_TimeSpan.ResumeDataUpdate();
            deviceMonitorService.AfterFetchPoint += (sender, e) => Chart_TimeSpan.SuspendDataUpdate();
        }
    }
}
