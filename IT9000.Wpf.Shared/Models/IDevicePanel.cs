using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IT9000.Wpf.Shared.Models
{
    public interface IDevicePanel
    {
        UIElement CreateDevicePanelUI(Device device);
        void Disconnect();
        void StartRunProgram(Device device);
        bool CanRunProgram(Device device);
        void StopRunProgram(Device device);
        bool CanStopProgram(Device device);
    }
}