using System;
using System.Collections.Generic;

namespace GSharp.Common.Extensions
{
    public interface IGControl : IGMetaData
    {
        IGExtension Parent { get; }

        IEnumerable<IGExportedData> Exports { get; }

        Type Source { get; }
    }
}
