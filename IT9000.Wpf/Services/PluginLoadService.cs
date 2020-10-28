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

        public void Load(Device device)
        {
            if(!_pluginsRepository.ModelTypeMap.ContainsKey(device.Model))
            {
                LoadDependency(device);
                foreach(Type type in Assembly.LoadFrom(GetPluginDllPath(device.Model)).GetTypes())
                {
                    Type? idevicePanelType = type.GetInterface(nameof(IDevicePanel));
                    if(idevicePanelType is not null)
                    { while(!_pluginsRepository.ModelTypeMap.TryAdd(device.Model,type)); }
                }
                
            }
        }

        private void LoadDependency(Device device)
        {
            IEnumerable<string> dllPaths = Directory.GetFiles(PluginsDirectory).Where(item=>item.EndsWith(".dll"));
            string devicePanelDllPath = GetPluginDllPath(device.Model);
            foreach(string dllPath in dllPaths)
            {
                if(dllPath!=devicePanelDllPath){
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                }
            }
        }
    }
}