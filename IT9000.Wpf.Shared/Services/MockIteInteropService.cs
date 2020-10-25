using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IT9000.Wpf.Shared.Services
{
    public class MockIteInteropService:IIteInteropService
    {
        public double SettingVolta { get; private set; }
        public double Volta { get; private set; }
        public double SettingAmpere { get; private set; }
        public double Ampere { get; private set; }
        public double VoltaMax { get; } = 100;
        public double VoltaMin { get; } = 0;
        public double AmpereMax { get; } = 20;
        public double AmpereMin { get; } = 0;
        public int OnlineFlag { get; private set; } = 0;
        public EventWaitHandle WaitHandle { get; } = new(true, EventResetMode.AutoReset);
        public int GetDeviceName(int address, byte[] ptr)
        {
            switch (address)
            {
                case 85711056: Encoding.Default.GetBytes("PV6900_1@1").CopyTo(ptr, 0); break;
                case 85682472: Encoding.Default.GetBytes("PV6900_2@2").CopyTo(ptr, 0); break;
                case 85682760: Encoding.Default.GetBytes("PV6900_3@3").CopyTo(ptr, 0); break;
                case 85683912: Encoding.Default.GetBytes("PV6900_4@4").CopyTo(ptr, 0); break;
                case 85686216: Encoding.Default.GetBytes("PV6900_5@5").CopyTo(ptr, 0); break;
                case 85687080: Encoding.Default.GetBytes("PV6900_6@6").CopyTo(ptr, 0); break;
                default: return 0;
            }
            return 1;
        }

        public int IteDC_GetUsb(byte[] ptr, ref int value)
        {
            value = 6;
            Encoding.Default.GetBytes("USB1").CopyTo(ptr, 0);
            Encoding.Default.GetBytes("USB2").CopyTo(ptr, 100);
            Encoding.Default.GetBytes("USB3").CopyTo(ptr, 200);
            Encoding.Default.GetBytes("USB4").CopyTo(ptr, 300);
            Encoding.Default.GetBytes("USB5").CopyTo(ptr, 400);
            Encoding.Default.GetBytes("USB6").CopyTo(ptr, 500);
            return 1;
        }

        public int IteDC_ReadCmd(int address, string scmd, byte[] reChar)
        {
            switch (scmd)
            {
                case "VOLT?":
                    Encoding.Default.GetBytes(SettingVolta.ToString()).CopyTo(reChar, 0); break;
                case "CURR?":
                    Encoding.Default.GetBytes(SettingAmpere.ToString()).CopyTo(reChar, 0); break;
                case "VOLT? MAX":
                    Encoding.Default.GetBytes(VoltaMax.ToString()).CopyTo(reChar, 0); break;
                case "VOLT? MIN":
                    Encoding.Default.GetBytes(VoltaMin.ToString()).CopyTo(reChar, 0); break;
                case "CURR? MAX":
                    Encoding.Default.GetBytes(AmpereMax.ToString()).CopyTo(reChar, 0); break;
                case "CURR? MIN":
                    Encoding.Default.GetBytes(AmpereMin.ToString()).CopyTo(reChar, 0); break;
            }
            return 1;
        }

        public int IteDC_WriteCmd(int address, string scmd)
        {
            if (scmd.StartsWith("VOLT"))
            {

                SettingVolta = double.Parse(scmd.Remove(0, 5));
                Volta = SettingVolta;
                return 1;
            }
            else if (scmd.StartsWith("CURR"))
            {

                SettingAmpere = double.Parse(scmd.Remove(0, 5));
                Ampere = SettingAmpere;
                return 1;
            }
            return 0;
        }

        public int IteDMM_GetMeasureCurrent(int address, string sChannel, byte[] reChar)
        {
            Encoding.Default.GetBytes(Ampere.ToString()).CopyTo(reChar, 0);
            return 1;
        }

        public int IteDMM_GetMeasureVoltage(int address, string sChannel, byte[] reChar)
        {
            Encoding.Default.GetBytes(Volta.ToString()).CopyTo(reChar, 0);
            return 1;
        }

        public int ItePow_GetOutputState(int address, byte[] reChar)
        {
            Encoding.Default.GetBytes($"{OnlineFlag}\n").CopyTo(reChar, 0);
            return 1;
        }

        public int ItePow_RemoteMode(int address)
        {
            return 0;
        }

        public int ItePow_SetOutputState(int address, string value)
        {
            OnlineFlag = int.Parse(value);
            return 1;
        }

        public int SystemTest(int[] ptr, ref int value)
        {
            value = 6;
            ptr[0] = 85711056;
            ptr[1] = 85682472;
            ptr[2] = 85682760;
            ptr[3] = 85683912;
            ptr[4] = 85686216;
            ptr[5] = 85687080;
            return 1;
        }
    }
}
