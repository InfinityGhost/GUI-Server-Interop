using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Library;
using Library.IPC;

namespace GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IAsyncInitialize
    {
        private Client Client;

        public async Task Initialize()
        {
            Console.WriteLine("Connecting to server...");
            Client = new Client();
            await Client.Initialize();
            Console.WriteLine("Connection to server established.");
        }

        public async Task TestButton()
        {
            await Client.PostAsync<string>(typeof(MainClass).GetMethod("SendMessage"), DateTime.Now.ToString());
        }

        delegate bool Test(string arg1);
    }
}
