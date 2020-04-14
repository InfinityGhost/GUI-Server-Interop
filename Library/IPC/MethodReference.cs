using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.IPC
{
    public class MethodReference
    {
        public MethodReference()
        {

        }
        
        public MethodReference(string path)
        {
            Path = path;
        }

        public MethodReference(LambdaExpression expr)
        {
            var methodExpr = expr.Body as MethodCallExpression;
            if (methodExpr == null)
                throw new NullReferenceException("Expression must reference a method.");
            var method = methodExpr.Method;
            Path = method.DeclaringType.FullName + "." + method.Name;
        }

        public MethodReference(MethodInfo method)
        {
            Path = method.DeclaringType.FullName + "." + method.Name;
        }

        public string Path { set; get; }

        public MethodInfo GetMethod()
        {
            var tokens = Path.Split('.');
            var methodName = tokens.Last();
            var className = string.Join('.', tokens[0..^1]);
            
            var type = Type.GetType(className, true);
            var method = type.GetMethod(methodName);
            return method;
        }
    }
}