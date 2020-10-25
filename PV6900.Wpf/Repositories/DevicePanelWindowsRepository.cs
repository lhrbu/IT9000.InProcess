using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using IT9000.Wpf.Shared.Models;

namespace PV6900.Wpf.Repositories
{
    public class DevicePanelWindowsRepository
    {
        public ConcurrentDictionary<Device, PV6900Window> DevicePanelWindowMap { get; }
            = new();
    }
}
