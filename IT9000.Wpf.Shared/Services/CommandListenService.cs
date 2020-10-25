using IT9000.Wpf.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT9000.Wpf.Shared.Services
{
    public class CommandListenService
    {
        private byte[] _buffer = new byte[256];
        public byte[] RentBuffer()
        {
            Array.Clear(_buffer, 0, 256);
            return _buffer;
        }
        private string GetPipeName(Device device) => $"Pipe_${device.Name}";

        public async Task ListenCommandAsync(Device device)
        {
            NamedPipeServerStream serverPipe = new(GetPipeName(device));
            await serverPipe.WaitForConnectionAsync();
            byte[] buffer = RentBuffer();
            int commandLength = await serverPipe.ReadAsync(buffer);
            string command = Encoding.UTF8.GetString(buffer.AsSpan(0, commandLength));
            OnReceivedCommand?.Invoke(this, new CommandReceivedEventArgs(command));
        }

        public event EventHandler<CommandReceivedEventArgs>? OnReceivedCommand;

    }
}
