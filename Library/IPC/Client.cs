using System;
using System.IO;
using System.IO.Pipes;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.IPC
{
    public class Client : IAsyncInitialize, IDisposable
    {
        public async Task Initialize()
        {
            await ClientStream.ConnectAsync();
            if (ClientStream.IsConnected)
            {
                ClientStreamWriter = new StreamWriter(ClientStream);
                ClientStreamReader = new StreamReader(ClientStream);
            }
            else
                throw new TimeoutException($"Client connection to server timed out after {Timeout}.");
        }

        public readonly TimeSpan Timeout = TimeSpan.FromSeconds(15);
        public NamedPipeClientStream ClientStream = new NamedPipeClientStream(
            ".",
            "GUI-Server-Interop",
            PipeDirection.InOut,
            PipeOptions.Asynchronous);
        private StreamWriter ClientStreamWriter;
        private StreamReader ClientStreamReader;

        private string Serialize<T>(MethodInfo method, params T[] arguments) where T : class
        {
            var methodRef = new MethodReference(method);
            var obj = new IPCMessage(methodRef, arguments);
            return JsonConvert.SerializeObject(obj);
        }

        public void Post<T>(MethodInfo method, params T[] arguments) where T : class
        {
            var json = Serialize(method, arguments);
            ClientStreamWriter.WriteLine(json);
        }

        public async Task PostAsync<T>(MethodInfo method, params T[] arguments) where T : class
        {
            var json = Serialize(method, arguments);
            await ClientStreamWriter.WriteLineAsync(json);
        }

        public void Dispose()
        {
            ClientStream?.Dispose();
            ClientStreamWriter?.Dispose();
        }
    }
}