﻿using PV6900.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Models;
using System.Windows;
using LiveCharts;
using LiveCharts.Configurations;

namespace PV6900.Wpf.ViewModels
{
    public class TimeSpanVoltaChartVM
    {
        static TimeSpanVoltaChartVM()
        {
            var mapper = Mappers.Xy<TimeSpanVoltaPoint>()
                .X(item => item.TimeSpan)
                .Y(item => item.Volta);
            Charting.For<TimeSpanVoltaPoint>(mapper);
        }

        private readonly DeviceMonitorService _deviceMonitorService;
        private readonly EventHandler<DataMeasureEventArgs> _onMeasured;
        public TimeSpanVoltaChartVM(DeviceMonitorService deviceMonitorService)
        {
            _deviceMonitorService = deviceMonitorService;
            _onMeasured = (sender, e) => FetchPoint(e.Volta);
            _deviceMonitorService.AfterDataMeasure +=  _onMeasured;
        }


        private DateTimeOffset _startTime = DateTimeOffset.Now;
        public ChartValues<TimeSpanVoltaPoint> Points { get; } = new();
        public void Reset()
        {
            Points.Clear();
            _startTime = DateTimeOffset.Now;
        }

        public void FetchPoint(double volta)
        {
            double timeSpan = (DateTimeOffset.Now - _startTime).TotalMilliseconds / 1000;
            TimeSpanVoltaPoint point = new()
            {
                TimeSpan = timeSpan,
                Volta = volta
            };
            
            if(Points.Count()>=25)
            {
                Points.RemoveAt(0);
            }
            Points.Add(point);
            
        }

        private bool _disposed = false;
        public void Dispose()
        {
           if (!_disposed)
           {
               _deviceMonitorService.AfterDataMeasure -= _onMeasured;
               _disposed = true;
           }
           GC.SuppressFinalize(this);
        }
    }
}
