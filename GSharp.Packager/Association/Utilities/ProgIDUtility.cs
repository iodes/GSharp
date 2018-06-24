using Microsoft.Win32;

namespace GSharp.Packager.Association.Utilities
{
    static class ProgIDUtility
    {
        #region 내부 함수
        private static void CreateType(string name, FileType type)
        {
            if (Registry.ClassesRoot.OpenSubKey(type.Extension, false) == null)
            {
                var typeKey = Registry.ClassesRoot.CreateSubKey(type.Extension);
                typeKey.SetValue(RegistryKeys.Default, name);
                typeKey.SetValue(RegistryKeys.ContentType, type.ContentType);
                typeKey.SetValue(RegistryKeys.PerceivedType, type.PerceivedType.ToString().ToLower());
            }
        }

        private static void CreateCommand(RegistryKey parentKey, ShellCommand command)
        {
            var shellKey = parentKey.CreateSubKey(RegistryKeys.Shell);
            var actionKey = shellKey.CreateSubKey(command.Action ?? "open");
            var cmdKey = actionKey.CreateSubKey(RegistryKeys.Command);

            var cmdStr = $"\"{command.Path}\" \"{command.Argument}\"";
            cmdKey.SetValue(RegistryKeys.Default, cmdStr);
        }
        #endregion

        #region 사용자 함수
        public static void CreateProgID(string name, ProgrammaticID prog)
        {
            // Prog ID 생성
            var progKey = Registry.ClassesRoot.CreateSubKey(name);
            progKey.SetValue(RegistryKeys.Default, prog.Description);
            progKey.SetValue(RegistryKeys.InfoTip, prog.InfoTip ?? prog.Description);

            var iconKey = progKey.CreateSubKey(RegistryKeys.Icon);
            iconKey.SetValue(RegistryKeys.Default, prog.Icon);

            // Prog ID 커멘드 생성
            CreateCommand(progKey, prog.Command);

            // 파일 타입 생성
            CreateType(name, prog.Type);
        }
        #endregion
    }
}
