using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV6900.Wpf.Services;

namespace PV6900.Wpf.ViewModels
{
    public class TimeSpanGaugesVM:BindableBase
    {
        private readonly DeviceMonitorService _deviceMonitorService;
        public TimeSpanGaugesVM(DeviceMonitorService deviceMonitorService)
        {
            _deviceMonitorService=deviceMonitorService;
            _deviceMonitorService.AfterSettingDataQuery += (sender, e) =>
            {
                  SettingVolta = e.SettingVolta;
                  SettingAmpere = e.SettingAmpere;
            };
            _deviceMonitorService.AfterDataMeasure += (sender, e) =>
             {
                Volta = e.Volta;
                Ampere = e.Ampere;
             };
        }
        public double Volta { get => _volta; set => SetProperty(ref _volta, value); }
        private double _volta;
        public double SettingVolta { get => _settingVolta; set => SetProperty(ref _settingVolta, value); }
        private double _settingVolta;
        public double Ampere { get => _ampere; set => SetProperty(ref _ampere, value); }
        private double _ampere;
        public double SettingAmpere { get => _settingAmpere; set => SetProperty(ref _settingAmpere, value); }
        private double _settingAmpere;
    }
}
