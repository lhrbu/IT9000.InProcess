using Prism.Commands;
using Prism.Mvvm;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Services;
using PV6900.Wpf.Views;
using Raccoon.DevKits.Wpf.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PV6900.Wpf.ViewModels
{
    public class MonitorMenuVM:BindableBase
    {
        private readonly DeviceMonitorService _deviceMonitorService;
        private readonly DeviceStorageService _deviceStorageService;
        private readonly TimeSpanChartsWindow _timeSpanChartsWindow;
        public MonitorMenuVM(
            DeviceMonitorService deviceMonitorService,
            DeviceStorageService deviceStorageService,
            TimeSpanChartsWindow timeSpanChartsWindow,
            TimeSpanVoltaChartVM timeSpanVoltaChartVM,
            TimeSpanAmpereChartVM timeSpanAmpereChartVM)
        { 
            _deviceMonitorService = deviceMonitorService;
            _deviceStorageService = deviceStorageService;
            _timeSpanChartsWindow = timeSpanChartsWindow;
            _timeSpanChartsWindow.Closing += (sender, e) =>
            {
                _timeSpanChartsWindow.Hide();
                timeSpanVoltaChartVM.Reset();
                timeSpanAmpereChartVM.Reset();
                e.Cancel=true;
            };

            StartMonitorCommand = new(StartMonitor);
            StopMonitorCommand = new(StopMonitor);
            ShowChartsCommand = new(ShowCharts);
        }

        public bool InMonitor
        { get => _inMonitor; set => SetProperty(ref _inMonitor, value); }
        private bool _inMonitor = false;
        public DelegateCommand StartMonitorCommand { get; }
        public DelegateCommand StopMonitorCommand { get; }
        public DelegateCommand<Window> ShowChartsCommand { get; }

        public void StartMonitor()
        {
            if (InMonitor || _deviceStorageService.Empty) { return; }
            InMonitor = true;
            _deviceMonitorService.StartAsync(_deviceStorageService.Get()!)
                .ContinueWith(task => InMonitor = false).ConfigureAwait(false);
        }
        public void StopMonitor()
        {
            if (!_deviceStorageService.Empty)
            { _deviceMonitorService.Stop(_deviceStorageService.Get()!); }
        }
        public void ShowCharts(Window window)
        {
            _timeSpanChartsWindow.Owner = Application.Current.MainWindow;
            _timeSpanChartsWindow.Show();
        }
    }
}
