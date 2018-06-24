using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace GSharp.Packager.Association.Utilities
{
    public static class AssociationUtility
    {
        #region 상수
        private const string keyDefault = "";
        private const string keyInfoTip = "InfoTip";
        private const string keyIcon = "DefaultIcon";
        #endregion

        #region 내부 함수
        private static void CreateProgID(string name, AssociationType type)
        {
            // 레지스트리 등록
            var progKey = Registry.ClassesRoot.CreateSubKey(name);
            progKey.SetValue(keyDefault, $"{name.Replace(".", " ")} File");
            progKey.SetValue(keyInfoTip, type.InfoTip);

            // 아이콘 설정
            var iconKey = progKey.CreateSubKey(keyIcon);
            iconKey.SetValue(keyDefault, type.Icon);
        }
        #endregion

        public static void Register(AssociationTarget target)
        {
            // Prog ID 등록
            foreach (var type in target.Associations)
            {
                var extName = type.Extension.Replace(".", "").ToUpper();
                var keyName = $"{target.CompanyName}.{target.DisplayName}.{extName}";

                CreateProgID(keyName, type);
            }
        }

        public static bool Launch(string appName = null)
        {
            var app = new ApplicationAssociationRegistrationUI() as IApplicationAssociationRegistrationUI;
            var hresult = app.LaunchAdvancedAssociationUI(appName);

            return Marshal.GetExceptionForHR(hresult) == null;
        }
    }
}
