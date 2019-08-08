using GSharp.Common.Extensions;

namespace GSharp.Common.Cores
{
    public interface IModule
    {
        IGCommand Command { get; }
    }
}
