using GSharp.Common.Extensions;
using System.Collections.Generic;

namespace GSharp.Common
{
    public interface IGMetaData
    {
        string Name { get; }

        string FriendlyName { get; }

        string NamespaceName { get; }

        string FullName { get; }

        IEnumerable<IGTranslation> Translations { get; }
    }
}
