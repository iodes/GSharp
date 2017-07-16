using GSharp.Extension.Attributes;
using System.Globalization;
using System.Linq;

namespace GSharp.Extension
{
    public class GTranslation
    {
        #region 속성
        public Locale Locale
        {
            get
            {
                return _Locale;
            }
        }
        private Locale _Locale;

        public string FriendlyName
        {
            get
            {
                return _FriendlyName;
            }
        }
        private string _FriendlyName;
        #endregion

        #region 생성자
        public GTranslation(string friendlyName, Locale locale)
        {
            _Locale = locale;
            _FriendlyName = friendlyName;
        }
        #endregion
    }

    public static class GTranslationSupport
    {
        public static GTranslation GetTranslation(GTranslation[] translations)
        {
            foreach (var translation in translations)
            {
                if (translation.Locale.ToString() == CultureInfo.CurrentUICulture.Name.ToUpper().Replace('-', '_'))
                {
                    return translation;
                }
            }

            return translations.First();
        }
    }
}
