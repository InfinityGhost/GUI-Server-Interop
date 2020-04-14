using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Library.IPC;
using System.Diagnostics;

namespace Library.IPC
{
    public class Server : IAsyncInitialize, IDisposable
    {
        public async Task Initialize()
        {
            await ServerStream.WaitForConnectionAsync();
            ServerStreamReader = new StreamReader(ServerStream);
            ServerStreamWriter = new StreamWriter(ServerStream);
            MainClass = new MainClass();
        }

        public async Task Main()
        {
            while (ServerStream.IsConnected)
            {
                var json = await ServerStreamReader.ReadLineAsync();
                IPCMessage data = JsonConvert.DeserializeObject<IPCMessage>(json);
                var method = data.TargetMethod.GetMethod();
                if (method.DeclaringType == typeof(MainClass))
                    method.Invoke(MainClass, data.Arguments);
            }
        }
        
        public MainClass MainClass;

        public NamedPipeServerStream ServerStream = new NamedPipeServerStream(
            "GUI-Server-Interop",
            PipeDirection.InOut,
            1,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);
        private StreamReader ServerStreamReader;
        private StreamWriter ServerStreamWriter;

        public void Dispose()
        {
            ServerStream?.Dispose();
            ServerStreamReader?.Dispose();
        }
    }
}