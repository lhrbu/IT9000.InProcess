using IT9000.Wpf.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT9000.Wpf.Shared.Services
{
    public class CommandSendService
    {
        private string GetPipeName(Device device) => $"Pipe_${device.Name}";
        public async ValueTask SendCommand(Device device,string command)
        {
            using NamedPipeClientStream clientPipe = new(GetPipeName(device));
            await clientPipe.ConnectAsync(3000);
            await clientPipe.WriteAsync(Encoding.UTF8.GetBytes(command));
        }
    }
}
