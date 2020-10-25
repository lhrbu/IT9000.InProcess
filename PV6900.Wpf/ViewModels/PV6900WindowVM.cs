using IT9000.Wpf.Shared.Models;
using Prism.Commands;
using Prism.Mvvm;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Raccoon.DevKits.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PV6900.Wpf.Views;
using System.Runtime.CompilerServices;

namespace PV6900.Wpf.ViewModels
{
    public class PV6900WindowVM:BindableBase
    {
        private readonly DeviceMonitorService _deviceMonitorService;
        private readonly TimeSpanAmpereChartVM _timeSpanAmpereChartVM;
        private readonly TimeSpanVoltaChartVM _timeSpanVoltaChartVM;
        public TimeSpanGaugesVM TimeSpanGaugesVM { get; }
        public ProgramDashboardVM ProgramDashboardVM { get; }
        public PV6900WindowVM(
            DeviceMonitorService deviceMonitorService,
            TimeSpanGaugesVM timeSpanGaugesVM,
            ProgramDashboardVM programDashboardVM,
            TimeSpanVoltaChartVM timeSpanVoltaChartVM,
            TimeSpanAmpereChartVM timeSpanAmpereChartVM
            )
        { 
            _deviceMonitorService = deviceMonitorService;
            TimeSpanGaugesVM = timeSpanGaugesVM;
            ProgramDashboardVM = programDashboardVM;
            _timeSpanVoltaChartVM = timeSpanVoltaChartVM;
            _timeSpanAmpereChartVM = timeSpanAmpereChartVM;

            StartMonitorCommand = new(StartMonitor);
            StopMonitorCommand = new(StopMonitor);
            ShowChartsCommand =new(ShowCharts);
        }

        public bool InMonitor
        { get => _inMonitor; set => SetProperty(ref _inMonitor, value); }
        private bool _inMonitor = false;
        public DelegateCommand StartMonitorCommand { get; }
        public DelegateCommand StopMonitorCommand { get; }
        public DelegateCommand<Window> ShowChartsCommand {get;}

        public void StartMonitor()
        {
            InMonitor = true;
            _startMonitorAwaitable = _deviceMonitorService.StartAsync().ConfigureAwait(false);
        }

        public void StopMonitor()
        {
            if(_startMonitorAwaitable.HasValue){
                _deviceMonitorService.Stop();
                _startMonitorAwaitable.Value.GetAwaiter().OnCompleted(()=>InMonitor=false);
            }
        }

        private ConfiguredTaskAwaitable? _startMonitorAwaitable;
        public void ShowCharts(Window window)
        {
            TimeSpanChartsWindow chartsWindow=new(_timeSpanVoltaChartVM,_timeSpanAmpereChartVM);
            chartsWindow.Owner = window;
            chartsWindow.Show();
        }
        
    }
}
