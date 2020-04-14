using System.Reflection;

namespace Library.IPC
{
    public class IPCMessage
    {
        public IPCMessage(MethodReference method, params object[] arguments)
        {
            TargetMethod = method;
            Arguments = arguments;
        }

        public MethodReference TargetMethod { set; get; }
        public object[] Arguments { set; get; }
    }
}