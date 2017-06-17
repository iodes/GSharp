using GSharp.Extension.Attributes;

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
            _Locale = Locale;
            _FriendlyName = friendlyName;
        }
        #endregion
    }
}
