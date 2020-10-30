using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT9000.Wpf.Shared.Models;

namespace IT9000.Wpf.Repositories
{
    public class PluginFactoriessRepository
    {
        public ConcurrentDictionary<string,IDevicePanelFactory> ModelFactoriesMap {get;}=new();
    }
}