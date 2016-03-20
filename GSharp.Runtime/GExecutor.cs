using System;
using System.Reflection;

namespace GSharp.Runtime
{
    public class GExecutor
    {
        #region 속성
        public string Path { get; set; }

        public string EntryPoint { get; set; }
        #endregion

        #region 객체
        private Type targetType;
        private object targetObject;
        private Assembly targetAssembly;
        #endregion

        #region 생성자
        public GExecutor(string pathValue, string entryPointValue)
        {
            Path = pathValue;
            EntryPoint = entryPointValue;

            targetAssembly = Assembly.LoadFrom(Path);
            targetType = targetAssembly.GetType(EntryPoint);
            targetObject = Activator.CreateInstance(targetType);
        }
        #endregion

        #region 사용자 함수
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

        public void CallMethod(string method, object[] parameter = null)
        {
            MethodInfo targetMethod = targetType.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            targetMethod.Invoke(targetObject, parameter == null ? null : parameter);
        }
        #endregion
    }
}
