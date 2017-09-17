namespace GSharp.Support.Utilities
{
    public static class EnvironmentUtility
    {
        public enum Environment
        {
            Wine,
            Native,
            Unknown
        }

        public static Environment GetEnvironment()
        {
            if (new WineEnvironment().IsEnvironment)
            {
                return Environment.Wine;
            }
            else if (new NTEnvironment().IsEnvironment)
            {
                return Environment.Native;
            }
            else
            {
                return Environment.Unknown;
            }
        }
    }
}
