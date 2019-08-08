using System;

namespace GSharp.Common.Optionals
{
    public interface IGOptional : IGMetaData
    {
        Type ObjectType { get; }
    }
}
