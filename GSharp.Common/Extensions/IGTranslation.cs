namespace GSharp.Common.Extensions
{
    public interface IGTranslation
    {
        LocaleType Locale { get; }

        string TranslatedName { get; }
    }
}
