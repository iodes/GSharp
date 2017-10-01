namespace GSharp.Support.Utilities
{
    public static class EnvironmentUtility
    {
        public enum Environment
        {
            Wine,
            Native,
            Virtual,
            Unknown
        }

        public static Environment GetEnvironment()
        {
            if (new WineEnvironment().IsEnvironment)
            {
                return Environment.Wine;
            }
            else if (new VirtualEnvironment().IsEnvironment)
            {
                return Environment.Virtual;
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
