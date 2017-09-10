using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Support.Utilities
{
    public static class EnvironmentUtility
    {
        public enum Environment
        {
            Wine,
            Native
        }

        public static Environment GetEnvironment()
        {
            return Environment.Native;
        }
    }
}
