using System.Runtime.InteropServices;

namespace GSharp.Packager.Association.Utilities
{
    public static class AssociationUtility
    {
        public static void Register(AssociationTarget target)
        {
            CreateSoftware(target);

            foreach (var type in target.Associations)
            {
                var extName = type.Extension.Replace(".", "").ToUpper();
                var keyName = $"{target.CompanyName}.{target.DisplayName}.{extName}";

                ProgIDUtility.CreateProgID(keyName, type);
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
