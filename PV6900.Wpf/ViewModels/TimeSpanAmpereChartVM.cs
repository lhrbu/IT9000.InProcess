using PV6900.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV6900.Wpf.Services;

namespace PV6900.Wpf.ViewModels
{
    public class TimeSpanAmpereChartVM
    {
        private readonly DeviceMonitorService _deviceMonitorService;
        public TimeSpanAmpereChartVM(DeviceMonitorService deviceMonitorService)
        {
            _deviceMonitorService = deviceMonitorService;
            _deviceMonitorService.AfterDataMeasure+=(sender,e)=>FetchPoint(e.Ampere);
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
            if (Points.Count() <= 50)
            {
                Points.Add(point);
            }
            else
            {
                Points.RemoveAt(0);
                Points.Add(point);
            }
        }
    }
}
