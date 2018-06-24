using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace GSharp.Packager.Association.Utilities
{
    public static class AssociationUtility
    {
        public static bool Register(Software target)
        {
            // Software 등록
            var softwarePath = $@"Software\{target.CompanyName}\{target.Name}\Capabilities";
            if (Registry.LocalMachine.OpenSubKey(softwarePath) == null)
            {
                var softwareKey = Registry.LocalMachine.CreateSubKey(softwarePath);
                softwareKey.SetValue(RegistryKeys.ApplicationName, target.Name);
                softwareKey.SetValue(RegistryKeys.ApplicationIcon, target.Icon);
                softwareKey.SetValue(RegistryKeys.ApplicationDescription, target.Description);

                var registeredKey = Registry.LocalMachine.OpenSubKey(@"Software\RegisteredApplications", true);
                if (registeredKey != null)
                {
                    registeredKey.SetValue(target.Name, softwarePath);
                }

                // Prog ID 등록
                var associationsKey = softwareKey.CreateSubKey("FileAssociations");
                foreach (var prog in target.Identifiers)
                {
                    var extName = prog.Type.Extension.Replace(".", "").ToUpper();
                    var keyName = $"{target.CompanyName}.{target.Name}.{extName}";

                    // Prog ID 생성
                    ProgIDUtility.CreateProgID(keyName, prog);

                    // Prog ID 연결
                    associationsKey.SetValue(prog.Type.Extension, keyName);
                }

                return true;
            }

            return false;
        }

        public static void Unregister(Software target)
        {
            var softwarePath = $@"Software\{target.CompanyName}\{target.Name}";
            if (Registry.LocalMachine.OpenSubKey(softwarePath) != null)
            {
                // Software 삭제
                Registry.LocalMachine.DeleteSubKeyTree(softwarePath);
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
