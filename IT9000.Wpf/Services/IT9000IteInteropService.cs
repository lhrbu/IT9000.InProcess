using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IT9000.Wpf.NativeInterop;
using IT9000.Wpf.Services;
using IT9000.Wpf.Shared.Services;

namespace IT9000.Wpf.Services
{
    public class IT9000IteInteropService : IIteInteropService
    {
        public EventWaitHandle WaitHandle => IT9000IteVirtualLayerInterop.WaitHandle;

        public int GetDeviceName(int address, byte[] ptr)
            => IT9000IteVirtualLayerInterop.GetDeviceName(address, ptr);

        public int IteDC_GetUsb(byte[] ptr, ref int value)
            => IT9000IteVirtualLayerInterop.IteDC_GetUsb(ptr, ref value);

        public int IteDC_ReadCmd(int address, string scmd, byte[] reChar)
            => IT9000IteVirtualLayerInterop.IteDC_ReadCmd(address, scmd, reChar);

        public int IteDC_WriteCmd(int address, string scmd)
            => IT9000IteVirtualLayerInterop.IteDC_WriteCmd(address, scmd);

        public int IteDMM_GetMeasureCurrent(int address, string sChannel, byte[] reChar)
            => IT9000IteVirtualLayerInterop.IteDMM_GetMeasureCurrent(address, sChannel, reChar);

        public int IteDMM_GetMeasureVoltage(int address, string sChannel, byte[] reChar)
            => IT9000IteVirtualLayerInterop.IteDMM_GetMeasureVoltage(address, sChannel, reChar);

        public int ItePow_GetOutputState(int address, byte[] reChar)
            => IT9000IteVirtualLayerInterop.ItePow_GetOutputState(address, reChar);
        public int ItePow_SetOutputState(int address, string value)
            => IT9000IteVirtualLayerInterop.ItePow_SetOutputState(address, value);

        public int SystemTest(int[] ptr, ref int value)
            => IT9000IteVirtualLayerInterop.SystemTest(ptr, ref value);
        public int ItePow_RemoteMode(int address)
            => IT9000IteVirtualLayerInterop.ItePow_RemoteMode(address);
    }
}
