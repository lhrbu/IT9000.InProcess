using Prism.Mvvm;
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
    public partial class PV6900Window : Window
    {
        private readonly PV6900WindowVM _pv6900WindowVM;
        public PV6900Window(
            PV6900WindowVM pv6900WindowVM,
            DeviceStorageService deviceStorageService
            )
        {
            _pv6900WindowVM = pv6900WindowVM;
            InitializeComponent();
            Title = deviceStorageService.Get()?.Name ?? "PV6900?";
            DataContext = _pv6900WindowVM;
            
        }
    }
}
