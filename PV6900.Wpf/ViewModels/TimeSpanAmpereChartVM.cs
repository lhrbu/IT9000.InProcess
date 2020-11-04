using PV6900.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Models;

namespace PV6900.Wpf.ViewModels
{
    public class TimeSpanAmpereChartVM:IDisposable
    {
        private readonly DeviceMonitorService _deviceMonitorService;
        private readonly EventHandler<DataMeasureEventArgs> _onMeaured;
        public TimeSpanAmpereChartVM(DeviceMonitorService deviceMonitorService)
        {
            _deviceMonitorService = deviceMonitorService;

            _onMeaured = (sender, e) => FetchPoint(e.Ampere);
            _deviceMonitorService.AfterDataMeasure+= _onMeaured;
        }
        private DateTimeOffset _startTime = DateTimeOffset.Now;
        public ObservableCollection<TimeSpanAmperePoint> Points { get; } = new();
        public void Reset()
        {
            Points.Clear();
            _startTime = DateTimeOffset.Now;
        }

        public void FetchPoint(double ampere)
        {
            TimeSpanAmperePoint point = new()
            {
                TimeSpan = (long)(DateTimeOffset.Now - _startTime).TotalSeconds,
                Ampere = ampere
            };
            if (Points.Count() <= 100)
            {
                Points.Add(point);
            }
            else
            {
                Points.RemoveAt(0);
                Points.Add(point);
            }
        }

        private bool _disposed = false;
        public void Dispose()
        {
           if(!_disposed)
           {
               _deviceMonitorService.AfterDataMeasure -= _onMeaured;
               _disposed = true;
           }
           GC.SuppressFinalize(this);
        }
    }
}
