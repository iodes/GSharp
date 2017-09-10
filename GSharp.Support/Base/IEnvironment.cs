namespace GSharp.Support.Base
{
    public interface IEnvironment
    {
        string Version { get; }

        bool IsEnvironment { get; }
    }
}
