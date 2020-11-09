using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using PV6900.Wpf.Shared.Models;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PV6900.Wpf.Services
{
    public class DeviceMonitorService
    {

        private readonly IIteInteropService _iteInteropService;
        private readonly DeviceLinkService _linkService;
        private readonly DeviceLimitsQueryService _limitsQueryService;
        private readonly DeviceDataMeasureService _measureService;
        private readonly DeviceSettingDataQueryService _settingDataQueryService;

        public DeviceMonitorService(
            IIteInteropService iteInteropService,
            DeviceLinkService linkService,
            DeviceLimitsQueryService limitsQueryService,
            DeviceDataMeasureService measureService,
            DeviceSettingDataQueryService settingDataQueryService)
        {
            _iteInteropService = iteInteropService;
            _linkService = linkService;
            _limitsQueryService = limitsQueryService;
            _measureService = measureService;
            _settingDataQueryService = settingDataQueryService;
        }

        public event EventHandler? BeforeFetchPoint;
        public event EventHandler<SettingDataQueryEventArgs>? AfterSettingDataQuery;
        public event EventHandler<DataMeasureEventArgs>? AfterDataMeasure;
        public event EventHandler? AfterFetchPoint;
        private readonly EventArgs _dummyArgs = new();
        public int IntervalMS { get; init; } = 300;

        private CancellationTokenSource? _cancellationTokenSource;

        public void Stop(Device device) => _cancellationTokenSource?.Cancel();
        public async Task StartAsync(Device device)
        {
            if(_cancellationTokenSource is not null && !_cancellationTokenSource.Token.IsCancellationRequested)
            { return; }


            if (!_linkService.TryLink(device))
            { throw new InvalidOperationException("Can't link device"); }

            _cancellationTokenSource = new();
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _iteInteropService.WaitHandle.WaitOne();
                double settingVolta = _settingDataQueryService.GetSettingVoltaUnsafe(device);
                double settingAmpere = _settingDataQueryService.GetSettingAmpereUnsafe(device);
                double volta = _measureService.GetVoltaUnsafe(device);
                double ampere = _measureService.GetAmpereUnsafe(device);
                _iteInteropService.WaitHandle.Set();
                BeforeFetchPoint?.Invoke(device, _dummyArgs);
                
                AfterSettingDataQuery?.Invoke(device, new SettingDataQueryEventArgs
                { SettingVolta = settingVolta, SettingAmpere = settingAmpere });
                AfterDataMeasure?.Invoke(device, new DataMeasureEventArgs
                { Volta = volta, Ampere = ampere });

                AfterFetchPoint?.Invoke(device, _dummyArgs);
                await Task.Delay(IntervalMS);
            }
        }
    }
}
