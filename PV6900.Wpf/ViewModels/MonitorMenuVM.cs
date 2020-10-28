using Prism.Commands;
using Prism.Mvvm;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Services;
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
        public MonitorMenuVM(
            DeviceMonitorService deviceMonitorService,
            DeviceStorageService deviceStorageService,
            TimeSpanVoltaChartVM timeSpanVoltaChartVM,
            TimeSpanAmpereChartVM timeSpanAmpereChartVM)
        { 
            _deviceMonitorService = deviceMonitorService;
            _deviceStorageService = deviceStorageService;

            StartMonitorCommand = new(StartMonitor);
            StopMonitorCommand = new(StopMonitor);
        }

        public bool InMonitor
        { get => _inMonitor; set => SetProperty(ref _inMonitor, value); }
        private bool _inMonitor = false;
        public DelegateCommand StartMonitorCommand { get; }
        public DelegateCommand StopMonitorCommand { get; }

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
    }
}
