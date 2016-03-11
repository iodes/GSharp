using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Runtime
{
    public class GExecutor
    {
        public string Path { get; set; }

        public string EntryPoint { get; set; }

        private Assembly targetAssembly;
        private Type targetType;
        private object targetObject;

        private void CallMethod(string method, object parameter = null)
        {
            MethodInfo targetMethod = targetType.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (parameter == null)
            {
                targetMethod.Invoke(targetObject, new object[] { });
            }
            else
            {
                targetMethod.Invoke(targetObject, new object[] { parameter });
            }
        }

        public GExecutor(string pathValue, string entryPointValue)
        {
            Path = pathValue;
            EntryPoint = entryPointValue;

            targetAssembly = Assembly.LoadFile(Path);
            targetType = targetAssembly.GetType(EntryPoint);
            targetObject = Activator.CreateInstance(targetType);
        }

        public void Execute()
        {
            try
            {
                CallMethod("Main");
            }
            catch (TargetInvocationException e)
            {
                Console.WriteLine("런타임 오류 : " + e.Message);
            }
        }
    }
}
