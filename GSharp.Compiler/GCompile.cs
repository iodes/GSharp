using System;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using GSharp.Cores;
using GSharp.Utilities;

namespace GSharp.Compiler
{
    public class GCompile
    {
        #region 객체
        private GScope Scope;
        private CSharpCodeProvider Provider;
        private CompilerParameters Parameters;
        #endregion

        #region 생성자
        public GCompile(GScope baseScope)
        {
            Scope = baseScope;
            Clean();
        }
        #endregion

        #region 내부 함수
        private void Clean()
        {
            Provider = new CSharpCodeProvider();
            Parameters = new CompilerParameters();
        }

        private void Initialize(string path)
        {
            Parameters.OutputAssembly = path;
            Parameters.ReferencedAssemblies.Add("System.dll");
            Parameters.ReferencedAssemblies.Add("System.Data.dll");
            Parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
        }

        private string ConvertToFullSource(string source)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("using System;");
            result.AppendLine("using System.Reflection;");
            result.AppendLine("using Microsoft.CSharp;");
            result.AppendLine();
            result.AppendLine("[assembly: AssemblyTitle(\"Title\")]");
            result.AppendLine("[assembly: AssemblyProduct(\"Product\")]");
            result.AppendLine("[assembly: AssemblyCompany(\"Company\")]");
            result.AppendLine("[assembly: AssemblyCopyright(\"Copyright\")]");
            result.AppendLine("[assembly: AssemblyTrademark(\"Trademark\")]");
            result.AppendLine("[assembly: AssemblyVersion(\"1.0.0.0\")]");
            result.AppendLine("[assembly: AssemblyFileVersion(\"1.0.0.0\")]");
            result.AppendLine();
            result.AppendLine("namespace MyNamespace");
            result.AppendLine("{");
            result.AppendLine("    class Program");
            result.AppendLine("    {");
            result.Append(ConvertAssistant.Indentation(source, 2));
            result.AppendLine("    }");
            result.AppendLine("}");

            return result.ToString();
        }
        #endregion

        #region 사용자 함수
        public CompilerResults Build(string path)
        {
            Clean();
            Initialize(path);

            string result = ConvertToFullSource(Scope.ToSource());
            Console.WriteLine(result);

            return Provider.CompileAssemblyFromSource(Parameters, result);
        }
        #endregion
    }
}
