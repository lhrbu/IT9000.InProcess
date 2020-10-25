using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Services
{
    public class DeviceSettingDataQueryService
    {
        private readonly IIteInteropService _iteInteropService;
        public DeviceSettingDataQueryService(IIteInteropService iteInteropService)
        { _iteInteropService = iteInteropService; }
        private byte[] _buffer = new byte[255];
        private byte[] RentBuffer()
        {
            Array.Clear(_buffer, 0, 255);
            return _buffer;
        }
        public double GetSettingVolta(Device device)
        {
            _iteInteropService.WaitHandle.WaitOne();
            double settingVolta = GetSettingVoltaUnsafe(device);
            _iteInteropService.WaitHandle.Set();
            return settingVolta;
        }
        public double GetSettingVoltaUnsafe(Device device)
        {
            byte[] settingVoltaBuffer = RentBuffer();
            _iteInteropService.IteDC_ReadCmd(device.Address, "VOLT?", settingVoltaBuffer);
            return double.Parse(Encoding.Default.GetString(settingVoltaBuffer).Trim(char.MinValue));
        }
        public double GetSettingAmpere(Device device)
        {
            _iteInteropService.WaitHandle.WaitOne();
            double settingAmpere = GetSettingAmpereUnsafe(device);
            _iteInteropService.WaitHandle.Set();
            return settingAmpere;
        }
        public double GetSettingAmpereUnsafe(Device device)
        {
            byte[] settingAmpereBuffer = RentBuffer();
            _iteInteropService.IteDC_ReadCmd(device.Address, "CURR?", settingAmpereBuffer);
            return double.Parse(Encoding.Default.GetString(settingAmpereBuffer).Trim(char.MinValue));
        }
    }
}
