using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.IPC
{
    public class Server : IDisposable
    {
        public Server()
        {
            MainClass = new MainClass();
            IsActive = true;
        }

        public async Task Connect()
        {
            await ServerStream.WaitForConnectionAsync();
            ServerStreamReader = new StreamReader(ServerStream);
            ServerStreamWriter = new StreamWriter(ServerStream);
        }

        public async Task Main()
        {
            await Connect();
            while (ServerStream.IsConnected)
            {
                try
                {
                    var json = await ServerStreamReader.ReadLineAsync();
                    IPCMessage data = JsonConvert.DeserializeObject<IPCMessage>(json);
                    var method = data.TargetMethod.GetMethod();
                    if (method.DeclaringType == typeof(MainClass))
                        method.Invoke(MainClass, data.Arguments);
                }
                catch (ArgumentNullException)
                {
                    ServerStream.Disconnect();
                    await Connect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        
        public MainClass MainClass;
        public bool IsActive { private set; get; }

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