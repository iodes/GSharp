using GSharp.Support.Base;
using System.Collections.Generic;
using System.Linq;

namespace GSharp.Support.Utilities
{
    public static class EnvironmentUtility
    {
        public enum EnvironmentType
        {
            Wine,
            Native,
            Virtual,
            Unknown
        }

        public static EnvironmentType Type
        {
            get
            {
                var currentEnv = GetEnvironment();

                if (currentEnv is WineEnvironment)
                {
                    return EnvironmentType.Wine;
                }
                else if (currentEnv is VirtualEnvironment)
                {
                    return EnvironmentType.Virtual;
                }
                else if (currentEnv is NTEnvironment)
                {
                    return EnvironmentType.Native;
                }
                else
                {
                    return EnvironmentType.Unknown;
                }
            }
        }

        public static IEnvironment GetEnvironment()
        {
            var listEnv = new List<IEnvironment>();

            listEnv.Add(new WineEnvironment());
            listEnv.Add(new VirtualEnvironment());
            listEnv.Add(new NTEnvironment());

            return listEnv.Where(x => x.IsEnvironment).FirstOrDefault();
        }
    }
}
