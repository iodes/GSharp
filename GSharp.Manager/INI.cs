using System.Text;
using System.Runtime.InteropServices;

namespace GSharp.Manager
{
    internal class INI
    {
        private string Path;

        public INI(string Path)
        {
            this.Path = Path;
        }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
            string section,
            string key,
            string def,
            StringBuilder retVal,
            int size,
            string filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(
             string section,
             string key,
             string val,
             string filePath);

        public string GetValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, Path);
            return temp.ToString();
        }

        public void SetValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }
    }
}
