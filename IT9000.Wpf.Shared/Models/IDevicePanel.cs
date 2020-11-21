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
        UIElement CreateDevicePanelUI();
        void Disconnect();
        void StartRunProgram();
        bool CanRunProgram();
        void StopRunProgram();
        bool CanStopProgram();
        bool CanStopMonitor();
        void StopMonitor();
    }
}