using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Services
{
    public class DeviceDataMeasureService
    {
        private readonly IIteInteropService _iteInteropService;
        private byte[] _buffer = new byte[255];
        private byte[] RentBuffer()
        {
            Array.Clear(_buffer, 0, 255);
            return _buffer;
        }
        public DeviceDataMeasureService(IIteInteropService iteInteropService)
        { _iteInteropService = iteInteropService; }
        public double GetVolta(Device device)
        {
            _iteInteropService.WaitHandle.WaitOne();
            double volta = GetVoltaUnsafe(device);
            _iteInteropService.WaitHandle.Set();
            return volta;
        }
        public double GetVoltaUnsafe(Device device)
        {
            byte[] voltaBuffer = RentBuffer();
            CheckErrNo(_iteInteropService.IteDMM_GetMeasureVoltage(device.Address, string.Empty, voltaBuffer));
            return double.Parse(Encoding.Default.GetString(voltaBuffer).Trim(char.MinValue));
        }
        public double GetAmpere(Device device)
        {
            _iteInteropService.WaitHandle.WaitOne();
            double ampere = GetAmpereUnsafe(device);
            _iteInteropService.WaitHandle.Set();
            return ampere;
        }
        public double GetAmpereUnsafe(Device device)
        {
            byte[] ampereBuffer = RentBuffer();
            CheckErrNo(_iteInteropService.IteDMM_GetMeasureCurrent(device.Address, string.Empty, ampereBuffer));
            return double.Parse(Encoding.Default.GetString(ampereBuffer).Trim(char.MinValue));
        }

        private void CheckErrNo(int errNo)
        {
            if (errNo != 1)
            {
                throw new InvalidOperationException("Wrong method used in DeviceMeasureService");
            }
        }
    }
}
