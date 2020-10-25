using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Services
{
    public class DeviceLinkService
    {

        private readonly IIteInteropService _iteInteropService;
        private byte[] _buffer = new byte[255];
        private byte[] RentBuffer()
        {
            Array.Clear(_buffer, 0, 255);
            return _buffer;
        }
        public DeviceLinkService(IIteInteropService iteInteropService)
        { _iteInteropService = iteInteropService; }

        public bool LinkCheck(Device device)
        {
            byte[] onLineFlagBuffer = RentBuffer();
            _iteInteropService.WaitHandle.WaitOne();
            _iteInteropService.ItePow_GetOutputState(device.Address, onLineFlagBuffer);
            _iteInteropService.WaitHandle.Set();
            string onLineFlag = Encoding.Default.GetString(onLineFlagBuffer).Trim(char.MinValue);
            return onLineFlag == "1\n";
        }
        public bool TryLink(Device device)
        {
            _iteInteropService.WaitHandle.WaitOne();
            _iteInteropService.IteDC_WriteCmd(device.Address, "SYST:REM");
            _iteInteropService.ItePow_SetOutputState(device.Address, "1");
            _iteInteropService.WaitHandle.Set();
            return true;
        }
        public bool TryUnlink(Device device)
        {
            _iteInteropService.WaitHandle.WaitOne();
            _iteInteropService.ItePow_RemoteMode(device.Address);
            _iteInteropService.ItePow_SetOutputState(device.Address, "0");
            _iteInteropService.WaitHandle.Set();
            return true;
        }
    }
}
