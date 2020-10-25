using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Services
{
    public class DeviceLimitsQueryService
    {
        private readonly IIteInteropService _iteInteropService;
        private byte[] _buffer = new byte[255];
        private byte[] RentBuffer()
        {
            Array.Clear(_buffer, 0, 255);
            return _buffer;
        }
        public DeviceLimitsQueryService(IIteInteropService iteInteropService)
        { _iteInteropService = iteInteropService; }
        public double GetVoltaMax(Device device)
        {
            byte[] voltaMaxBuffer = RentBuffer();
            _iteInteropService.IteDC_ReadCmd(device.Address, "VOLT? MAX", voltaMaxBuffer);

            return double.Parse(Encoding.Default.GetString(voltaMaxBuffer).Trim(char.MinValue));
        }
        public double GetVoltaMin(Device device)
        {
            byte[] voltaMinBuffer = RentBuffer();
            _iteInteropService.IteDC_ReadCmd(device.Address, "VOLT? MIN", voltaMinBuffer);
            return double.Parse(Encoding.Default.GetString(voltaMinBuffer).Trim(char.MinValue));
        }

        public double GetAmpereMax(Device device)
        {
            byte[] ampereMaxBuffer = RentBuffer();
            _iteInteropService.IteDC_ReadCmd(device.Address, "CURR? MAX", ampereMaxBuffer);
            return double.Parse(Encoding.Default.GetString(ampereMaxBuffer).Trim(char.MinValue));
        }
        public double GetAmpereMin(Device device)
        {
            byte[] ampereMinBuffer = RentBuffer();
            _iteInteropService.IteDC_ReadCmd(device.Address, "CURR? MIN", ampereMinBuffer);
            return double.Parse(Encoding.Default.GetString(ampereMinBuffer).Trim(char.MinValue));
        }
    }
}
