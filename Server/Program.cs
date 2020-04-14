using System;
using System.Threading.Tasks;
using Library.IPC;
using IPCServer = Library.IPC.Server;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Waiting for a client to connect...");
            Server = new IPCServer();
            await Server.Initialize();
            Console.WriteLine("Client has successfully connected.");

            while (Server.ServerStream.IsConnected)
            {
                await Server.Main();
            }
        }

        static IPCServer Server;
    }
}
