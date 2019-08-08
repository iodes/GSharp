using System.Collections.Generic;

namespace GSharp.Common.Extensions
{
    public interface IGExtension
    {
        string Path { get; set; }

        string Title { get; set; }

        string Author { get; set; }

        string Summary { get; set; }

        string Namespace { get; set; }

        string Dependencies { get; set; }

        IEnumerable<IGCommand> Commands { get; set; }

        IEnumerable<IGControl> Controls { get; set; }
    }
}
