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
    public class TimeSpanVoltaChartVM
    {
        private readonly DeviceMonitorService _deviceMonitorService;
        //private readonly EventHandler<DataMeasureEventArgs> _onDataMeasured;
        public TimeSpanVoltaChartVM(DeviceMonitorService deviceMonitorService)
        {
            _deviceMonitorService = deviceMonitorService;
            //_onDataMeasured = (sender, e) => FetchPoint(e.Volta);
            _deviceMonitorService.AfterDataMeasure += (sender, e) => FetchPoint(e.Volta);
        }


        private DateTimeOffset _startTime = DateTimeOffset.Now;
        public ObservableCollection<TimeSpanVoltaPoint> Points { get; }= new();
        public void Reset()
        {
            Points.Clear();
            _startTime = DateTimeOffset.Now;
        }

        public void FetchPoint(double volta)
        {
            TimeSpanVoltaPoint point = new()
            {
                TimeSpan = (long)(DateTimeOffset.Now - _startTime).TotalSeconds,
                Volta = volta
            };
            if (Points.Count() <= 60)
            {
                Points.Add(point);
            }
            else
            {
                Points.RemoveAt(0);
                Points.Add(point);
            }
        }

        //private bool _disposed = false;
        //public void Dispose()
        //{
        //    if (!_disposed)
        //    {
        //        _deviceMonitorService.AfterDataMeasure -= _onDataMeasured;
        //        _disposed = true;
        //    }
        //    GC.SuppressFinalize(this);
        //}
    }
}
