using System;
using GSharp.Common.Optionals;
using System.Collections.Generic;

namespace GSharp.Common.Extensions
{
    public interface IGExportedData : IGMetaData
    {
        IEnumerable<IGOptional> Optionals { get; }
        
        string MethodName { get; }
        
        Type ObjectType { get; }
    }
}
