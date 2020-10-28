using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using IT9000.Wpf.Repositories;
using System.Reflection;
using System.IO;
using System.Runtime.Loader;
using System.Runtime.CompilerServices;

namespace IT9000.Wpf.Services
{
    public class PluginLoadService
    {
        private readonly PluginTypesRepository _pluginsRepository;
        public PluginLoadService(PluginTypesRepository pluginsRepository)
        { _pluginsRepository=pluginsRepository;}
        private string PluginsDirectory => Path.Combine(
            Environment.CurrentDirectory, "Plugins");
        private string GetPluginDllPath(string deviceModel) => Path.Combine(
            PluginsDirectory, $"{deviceModel}.Wpf.dll");

        private string GetPluginDependencyDirectory(string deviceModel)=>Path
            .Combine(PluginsDirectory,$"{deviceModel}.Refs");

        public void Load(Device device)
        {
            if(!_pluginsRepository.ModelTypeMap.ContainsKey(device.Model))
            {
                //LoadDependency(device);
                //ConfiguredTaskAwaitable awaitable = Task.Run(()=>LoadDependency(device)).ConfigureAwait(false);
                foreach(Type type in Assembly.LoadFrom(GetPluginDllPath(device.Model)).GetTypes())
                {
                    Type? idevicePanelType = type.GetInterface(nameof(IDevicePanel));
                    if(idevicePanelType is not null)
                    { _pluginsRepository.ModelTypeMap.TryAdd(device.Model,type); }
                }
                //awaitable.GetAwaiter().GetResult();
            }
        }

        public void LoadDependency(Device device)
        {
            if (!_pluginsRepository.ModelTypeMap.ContainsKey(device.Model))
            {
                IEnumerable<string> dllPaths = Directory.GetFiles(GetPluginDependencyDirectory(device.Model)).Where(item => item.EndsWith(".dll"));
                foreach (string dllPath in dllPaths)
                { AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath); }
            }
        }
    }
}