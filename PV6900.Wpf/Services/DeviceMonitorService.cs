using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using PV6900.Wpf.Shared.Models;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Services
{
    public class DeviceMonitorService
    {
        private readonly DeviceStorageService _deviceStorageService;

        private readonly IIteInteropService _iteInteropService;
        private readonly DeviceLinkService _linkService;
        private readonly DeviceLimitsQueryService _limitsQueryService;
        private readonly DeviceDataMeasureService _measureService;
        private readonly DeviceSettingDataQueryService _settingDataQueryService;

        public DeviceMonitorService(
            DeviceStorageService deviceStorageService,
            IIteInteropService iteInteropService,
            DeviceLinkService linkService,
            DeviceLimitsQueryService limitsQueryService,
            DeviceDataMeasureService measureService,
            DeviceSettingDataQueryService settingDataQueryService)
        {
            _deviceStorageService = deviceStorageService;
            _iteInteropService = iteInteropService;
            _linkService = linkService;
            _limitsQueryService = limitsQueryService;
            _measureService = measureService;
            _settingDataQueryService = settingDataQueryService;
        }


        public event EventHandler<SettingDataQueryEventArgs>? AfterSettingDataQuery;
        public event EventHandler<DataMeasureEventArgs>? AfterDataMeasure;

        public int IntervalMS { get; init; } = 100;
        private bool _isRequestCancellation = false;
        public void Stop() => _isRequestCancellation = true;
        public async Task StartAsync()
        {
            if (_deviceStorageService.Empty) { return; }
            Device device = _deviceStorageService.Get()!;


            if (!_linkService.TryLink(device))
            { throw new InvalidOperationException("Can't link device"); }

            _isRequestCancellation=false;
            while (!_isRequestCancellation)
            {
                _iteInteropService.WaitHandle.WaitOne();
                double settingVolta = _settingDataQueryService.GetSettingAmpereUnsafe(device);
                double settingAmpere = _settingDataQueryService.GetSettingAmpereUnsafe(device);
                double volta = _measureService.GetVoltaUnsafe(device);
                double ampere = _measureService.GetAmpereUnsafe(device);
                _iteInteropService.WaitHandle.Set();

                AfterSettingDataQuery?.Invoke(device, new SettingDataQueryEventArgs
                { SettingVolta = settingVolta, SettingAmpere = settingAmpere });
                AfterDataMeasure?.Invoke(device, new DataMeasureEventArgs
                { Volta = volta, Ampere = ampere });
                await Task.Delay(IntervalMS);
            }
        }
    }
}
