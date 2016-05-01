using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.CSharp;
using GSharp.Base.Utilities;

namespace GSharp.Compile
{
    public class GCompiler
    {
        #region 속성
        public string Source { get; set; }

        public StringCollection References
        {
            get
            {
                return parameters.ReferencedAssemblies;
            }
        }
        #endregion

        #region 객체
        private CSharpCodeProvider provider = new CSharpCodeProvider();
        private CompilerParameters parameters = new CompilerParameters();
        #endregion

        #region 생성자
        public GCompiler()
        {
            foreach (string reference in GetDefaultReference())
            {
                References.Add(reference);
            }
        }

        public GCompiler(string value) : this()
        {
            Source = value;
        }
        #endregion

        #region 내부 함수
        private bool IsNameContains(StringCollection references, string name)
        {
            return (from string dll
                    in references
                    where Path.GetFileName(dll) == Path.GetFileName(name)
                    select dll).ToArray().Count() > 0;
        }

        private string GetPublicKeyToken(AssemblyName assembly)
        {
            StringBuilder builder = new StringBuilder();
            byte[] token = assembly.GetPublicKeyToken();

            for (int i = 0; i < token.GetLength(0); i++)
            {
                builder.AppendFormat("{0:x2}", token[i]);
            }

            return builder.ToString();
        }

        private List<string> GetDefaultReference()
        {
            List<string> result = new List<string>();

            result.Add("System.dll");
            result.Add("System.Linq.dll");
            result.Add("System.Windows.Forms.dll");

            return result;
        }

        private string ConvertToFullSource(string source)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("using System;");
            result.AppendLine("using System.Collections.Generic;");
            result.AppendLine("using System.Linq;");
            result.AppendLine("using System.Text;");
            result.AppendLine("using System.Threading.Tasks;");
            result.AppendLine("using System.Reflection;");
            result.AppendLine("using System.Windows.Forms;");
            result.AppendLine();
            result.AppendLine("[assembly: AssemblyTitle(\"Title\")]");
            result.AppendLine("[assembly: AssemblyProduct(\"Product\")]");
            result.AppendLine("[assembly: AssemblyCompany(\"Company\")]");
            result.AppendLine("[assembly: AssemblyCopyright(\"Copyright\")]");
            result.AppendLine("[assembly: AssemblyTrademark(\"Trademark\")]");
            result.AppendLine("[assembly: AssemblyVersion(\"1.0.0.0\")]");
            result.AppendLine("[assembly: AssemblyFileVersion(\"1.0.0.0\")]");
            result.AppendLine();
            result.AppendLine("namespace GSharp.Scenario");
            result.AppendLine("{");
            result.AppendLine("    class Default");
            result.AppendLine("    {");
            result.AppendLine("        [STAThread]");
            result.AppendLine("        static void Main(string[] args)");
            result.AppendLine("        {");
            result.AppendLine("            Application.EnableVisualStyles();");
            result.AppendLine("            Application.SetCompatibleTextRenderingDefault(false);");
            result.AppendLine("            Application.Run(new DefaultForm());");
            result.AppendLine("        }");
            result.AppendLine("    }");
            result.AppendLine();
            result.AppendLine("    public partial class DefaultForm : Form");
            result.AppendLine("    {");
            result.AppendLine("        public DefaultForm()");
            result.AppendLine("        {");
            result.AppendLine("            Text = \"GSharp Runtime\";");
            result.AppendLine("            MaximizeBox = false;");
            result.AppendLine("            MinimizeBox = false;");
            result.AppendLine("            StartPosition = FormStartPosition.CenterScreen;");
            result.AppendLine("            FormBorderStyle = FormBorderStyle.FixedSingle;");
            result.AppendLine("            Initialize();");
            result.AppendLine("        }");
            result.AppendLine();
            result.Append(ConvertAssistant.Indentation(source, 2));
            result.AppendLine("    }");
            result.AppendLine("}");

            return result.ToString();
        }
        #endregion

        #region 사용자 함수
        /// <summary>
        /// 외부 참조를 추가합니다.
        /// </summary>
        /// <param name="path">외부 참조 파일의 경로입니다.</param>
        public void LoadReference(string path)
        {
            // 참조 추가
            if (!IsNameContains(References, path))
            {
                References.Add(path);
            }

            // 참조의 종속성 검사
            foreach (AssemblyName assembly in Assembly.LoadFrom(path).GetReferencedAssemblies())
            {
                // 참조 종속성 검사
                string referencesName = null;
                string dllPath = string.Format(@"{0}\{1}.dll", Path.GetDirectoryName(path), assembly.Name);
                if (File.Exists(dllPath))
                {
                    // 동일 경로에 존재
                    referencesName = dllPath;
                }
                else
                {
                    // 동일 경로에 없음
                    // 외부 종속성 중복 검사
                    if ((from callingAssembly
                         in Assembly.GetCallingAssembly().GetReferencedAssemblies()
                         select callingAssembly.Name).Contains(assembly.Name))
                    {
                        continue;
                    }

                    // 글로벌 캐시에 존재 여부 검사
                    string[] dllGAC =
                        Directory.GetFiles
                        (
                            Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\assembly", assembly.Name + ".dll",
                            SearchOption.AllDirectories
                        );
                    dllGAC = dllGAC.Where(dll => dll.IndexOf(GetPublicKeyToken(assembly)) != -1).ToArray();

                    if (dllGAC.Length > 0)
                    {
                        // 글로벌 캐시에 존재
                        // 시스템에 맞는 파일 검색
                        if (dllGAC.Length == 1)
                        {
                            referencesName = dllGAC.First();
                        }
                        else
                        {
                            referencesName = dllGAC.Where(dll => dll.IndexOf(Environment.Is64BitOperatingSystem ? "GAC_64" : "GAC_32") != -1).First();
                        }
                    }
                    else
                    {
                        referencesName = assembly.Name + ".dll";
                    }
                }

                // 참조의 종속성을 추가
                if (!IsNameContains(References, referencesName))
                {
                    References.Add(referencesName);
                }
            }
        }

        /// <summary>
        /// 소스를 빌드하여 컴파일된 파일을 생성합니다.
        /// </summary>
        /// <param name="path">컴파일된 파일을 생성할 경로입니다.</param>
        /// <param name="isExecutable">실행 파일 형태로 컴파일 할지 여부를 설정합니다.</param>
        /// <returns></returns>
        public GCompilerResults Build(string path, bool isExecutable = false)
        {
            parameters.OutputAssembly = path;
            parameters.GenerateExecutable = isExecutable;
            parameters.CompilerOptions = "/platform:x86 /target:winexe";
            string fullSource = ConvertToFullSource(Source);

            GCompilerResults results = new GCompilerResults
            {
                Source = fullSource,
                Results = provider.CompileAssemblyFromSource(parameters, fullSource)
            };

            foreach (string dll in References)
            {
                if (File.Exists(dll))
                {
                    File.Copy(dll, string.Format(@"{0}\{1}", Path.GetDirectoryName(path), Path.GetFileName(dll)), true);
                }
            }

            return results;
        }
        #endregion
    }
}
