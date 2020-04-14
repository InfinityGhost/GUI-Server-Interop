using System;
using System.Diagnostics;

namespace Library
{
    public class MainClass
    {
        public void Initialize()
        {
            Console.WriteLine("Initialized!");
        }
        
        public bool SendMessage(string message)
        {
            Console.WriteLine(message);
            return true;
        }

        public void MultipleArgumentsTest(string message, int num, object generic)
        {
            Console.WriteLine($"Received: {message}, {num}, {generic}");
        }
    }
}
