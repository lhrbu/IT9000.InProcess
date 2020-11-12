using PV6900.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Models;
using LiveCharts;
using LiveCharts.Configurations;

namespace PV6900.Wpf.ViewModels
{
    public class TimeSpanAmpereChartVM:IDisposable
    {
        static TimeSpanAmpereChartVM()
        {
            var mapper = Mappers.Xy<TimeSpanAmperePoint>()
               .X(item => item.TimeSpan)
               .Y(item => item.Ampere);
            Charting.For<TimeSpanAmperePoint>(mapper);
        }

        private readonly DeviceMonitorService _deviceMonitorService;
        private readonly EventHandler<DataMeasureEventArgs> _onMeaured;
        public TimeSpanAmpereChartVM(DeviceMonitorService deviceMonitorService)
        {
            _deviceMonitorService = deviceMonitorService;

            _onMeaured = (sender, e) => FetchPoint(e.Ampere);
            _deviceMonitorService.AfterDataMeasure+= _onMeaured;
        }
        private DateTimeOffset _startTime = DateTimeOffset.Now;
        public ChartValues<TimeSpanAmperePoint> Points { get; } = new();
        public void Reset()
        {
            Points.Clear();
            _startTime = DateTimeOffset.Now;
        }

        public void FetchPoint(double ampere)
        {
            TimeSpanAmperePoint point = new()
            {
                TimeSpan = (DateTimeOffset.Now - _startTime).TotalMilliseconds / 1000,
                Ampere = ampere
            };

            if (Points.Count() >= 25)
            {
                Points.RemoveAt(0);
            }
            Points.Add(point);
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
