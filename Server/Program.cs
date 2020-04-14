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
            Server = new IPCServer();
            
            while (Server.IsActive)
            {
                await Server.Main();
            }
        }

        static IPCServer Server;
    }
}
