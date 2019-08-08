using GSharp.Common.Optionals;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GSharp.Common.Extensions
{
    public interface IGCommand : IGMetaData
    {
        IGExtension Parent { get; }

        IEnumerable<IGOptional> Optionals { get; }

        Type ObjectType { get; }

        string MethodName { get; }

        CommandType MethodType { get; }

        Color BlockColor { get; }
    }
}
