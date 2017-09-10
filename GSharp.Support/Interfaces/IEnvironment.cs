namespace GSharp.Support.Interfaces
{
    public interface IEnvironment
    {
        string Version { get; }

        bool IsEnvironment { get; }
    }
}
