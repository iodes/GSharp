namespace GSharp.Packager.Commons
{
    public interface IValueAttribute<T>
    {
        T Value { get; set; }
    }
}
