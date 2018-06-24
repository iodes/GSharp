using Microsoft.Win32;

namespace GSharp.Packager.Association.Utilities
{
    static class ProgIDUtility
    {
        private static void CreateCommand(RegistryKey parentKey, ShellCommand command)
        {
            var shellKey = parentKey.CreateSubKey("shell");
            var openKey = shellKey.CreateSubKey("open");
            var cmdKey = openKey.CreateSubKey("command");

            var cmdStr = $"\"{command.Path}\" \"{command.Argument}\"";
            cmdKey.SetValue(RegistryKeys.Default, cmdStr);
        }

        public static void CreateProgID(string name, AssociationType type)
        {
            var progKey = Registry.ClassesRoot.CreateSubKey(name);
            progKey.SetValue(RegistryKeys.Default, $"{name.Replace(".", " ")} File");
            progKey.SetValue(RegistryKeys.InfoTip, type.InfoTip);

            var iconKey = progKey.CreateSubKey(RegistryKeys.Icon);
            iconKey.SetValue(RegistryKeys.Default, type.Icon);

            CreateCommand(progKey, type.Command);
        }
    }
}
