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
        private List<string> GetDefaultReference()
        {
            List<string> result = new List<string>();

            result.Add("System.dll");
            result.Add("System.Linq.dll");

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
            References.Add(path);
            foreach (AssemblyName assembly in Assembly.LoadFrom(path).GetReferencedAssemblies())
            {
                if (!References.Contains(assembly.Name))
                {
                    References.Add(assembly.Name + ".dll");
                }
            }
        }

        /// <summary>
        /// 소스를 빌드하여 컴파일된 파일을 생성합니다.
        /// </summary>
        /// <param name="path">컴파일된 파일을 생성할 경로입니다.</param>
        public GCompilerResults Build(string path)
        {
            parameters.OutputAssembly = path;
            string fullSource = ConvertToFullSource(Source);

            GCompilerResults results = new GCompilerResults
            {
                Source = fullSource,
                Results = provider.CompileAssemblyFromSource(parameters, fullSource)
            };

            return results;
        }
        #endregion
    }
}
