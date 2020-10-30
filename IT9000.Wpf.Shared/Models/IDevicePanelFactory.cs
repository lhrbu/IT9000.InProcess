using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT9000.Wpf.Shared.Models
{
    public interface IDevicePanelFactory
    {
        IDevicePanel CreateDevicePanel(Device device);
    }
}