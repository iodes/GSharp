using System.Collections.ObjectModel;

namespace GSharp.Packager.Commons
{
    public sealed class ReadOnlyPackageDataCollection : ReadOnlyObservableCollection<IPackageData>
    {
        internal ReadOnlyPackageDataCollection(ObservableCollection<IPackageData> list) : base(list)
        {

        }
    }
}
