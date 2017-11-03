using GSharp.Base.Utilities;
using GSharp.Compile.Properties;
using GSharp.Compressor;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSharp.Compile
{
    public class GCompiler
    {
        #region 속성
        /// <summary>
        /// 컴파일 대상의 소스입니다.
        /// </summary>
        public string Source { get; set; }

        public StringCollection References
        {
            get
            {
                return parameters.ReferencedAssemblies;
            }
        }

        /// <summary>
        /// 컴파일 대상 창의 디자인 소스입니다.
        /// </summary>
        public string XAML
        {
            get
            {
                return Base64Decode(_XAML);
            }
            set
            {
                _XAML = Base64Encode(value);
            }
        }
        private string _XAML;

        /// <summary>
        /// 사용자가 불러온 참조의 목록입니다.
        /// </summary>
        public List<string> LoadedReferences { get; set; } = new List<string>();

        /// <summary>
        /// 컴파일시 복사될 의존성의 목록입니다.
        /// </summary>
        public List<string> Dependencies { get; set; } = new List<string>();
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
        private string Base64Encode(string plainText)
        {
            if (plainText?.Length > 0)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            }
            else
            {
                return string.Empty;
            }
        }

        private string Base64Decode(string base64EncodedData)
        {
            if (base64EncodedData?.Length > 0)
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
            }
            else
            {
                return string.Empty;
            }
        }

        private bool IsNameContains(StringCollection references, string name)
        {
            lock (references)
            {
                return (from string dll in references where Path.GetFileName(dll) == Path.GetFileName(name) select dll).Count() > 0;
            }
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
            result.Add("System.Core.dll");
            result.Add("System.Data.dll");
            result.Add("System.Data.DataSetExtensions.dll");
            result.Add("System.Net.Http.dll");
            result.Add("System.Xml.dll");
            result.Add("System.Xml.Linq.dll");
            result.Add("System.Linq.dll");

            var resources = Resources.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true);
            foreach (DictionaryEntry resource in resources)
            {
                var path = Path.GetTempPath() + $"{(resource.Key as string).Replace('_', '.')}.dll";
                File.WriteAllBytes(path, resource.Value as byte[]);
                result.Add(path);
            }

            return result;
        }

        private string ConvertToFullSource(string source, GCompilerConfig config = null)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("using System;");
            result.AppendLine("using System.IO;");
            result.AppendLine("using System.Collections.Generic;");
            result.AppendLine("using System.Linq;");
            result.AppendLine("using System.Text;");
            result.AppendLine("using System.Threading.Tasks;");
            result.AppendLine("using System.Reflection;");
            result.AppendLine("using System.Windows;");
            result.AppendLine("using System.Windows.Markup;");
            result.AppendLine("using GSharp.Compressor;");
            result.AppendLine("using GSharp.Bootstrap.DataTypes;");
            result.AppendLine();
            result.AppendLine($"[assembly: AssemblyTitle(\"{config.Title}\")]");
            result.AppendLine($"[assembly: AssemblyDescription(\"{config.Description}\")]");
            result.AppendLine($"[assembly: AssemblyCompany(\"{config.Company}\")]");
            result.AppendLine($"[assembly: AssemblyProduct(\"{config.Product}\")]");
            result.AppendLine($"[assembly: AssemblyCopyright(\"{config.Copyright}\")]");
            result.AppendLine($"[assembly: AssemblyTrademark(\"{config.Trademark}\")]");
            result.AppendLine($"[assembly: AssemblyVersion(\"{config.Version}\")]");
            result.AppendLine($"[assembly: AssemblyFileVersion(\"{config.FileVersion}\")]");
            result.AppendLine();
            result.AppendLine("namespace GSharp.Default");
            result.AppendLine("{");
            result.AppendLine("    public partial class App : Application");
            result.AppendLine("    {");
            if (XAML.Length > 0)
            {
                result.AppendLine("        public static Window window;");
                result.AppendLine();
            }
            if (config.IsEmbedded && config.IsCompressed)
            {
                result.AppendLine("        public static Assembly compressor;");
                result.AppendLine("        public static MethodInfo compressorMethod;");
                result.AppendLine();
            }
            result.AppendLine("        [STAThread]");
            result.AppendLine("        public static void Main()");
            result.AppendLine("        {");
            if (config.IsEmbedded || config.IsCompressed)
            {
                result.AppendLine("            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Resolve);");
                if (config.IsEmbedded && config.IsCompressed)
                {
                    result.AppendLine();
                    result.AppendLine("            var current = Assembly.GetExecutingAssembly();");
                    result.AppendLine(@"            var files = current.GetManifestResourceNames().Where(s => s.EndsWith(""GSharp.Compressor.dll""));");
                    result.AppendLine("            using (var stream = current.GetManifestResourceStream(files.First()))");
                    result.AppendLine("            {");
                    result.AppendLine("                var data = new byte[stream.Length];");
                    result.AppendLine("                stream.Read(data, 0, data.Length);");
                    result.AppendLine("                compressor = Assembly.Load(data);");
                    result.AppendLine(@"                compressorMethod = compressor.GetType(""GSharp.Compressor.GCompressor"").GetMethod(""Decompress"", new Type[] { typeof(Stream) });");
                    result.AppendLine("            }");
                    result.AppendLine();
                }
            }
            result.AppendLine("            App app = new App();");
            result.AppendLine("            app.InitializeComponent();");
            result.AppendLine("            app.Run();");
            result.AppendLine("        }");
            if (config.IsEmbedded || config.IsCompressed)
            {
                result.AppendLine();
                result.AppendLine("        public static Assembly Resolve(object sender, ResolveEventArgs args)");
                result.AppendLine("        {");
                result.AppendLine("            var current = Assembly.GetExecutingAssembly();");
                if (config.IsCompressed)
                {
                    result.AppendLine(@"            var name = args.Name.Substring(0, args.Name.IndexOf(',')) + "".pak"";");
                }
                else
                {
                    result.AppendLine(@"            var name = args.Name.Substring(0, args.Name.IndexOf(',')) + "".dll"";");
                }

                if (config.IsEmbedded)
                {
                    result.AppendLine("            var files = current.GetManifestResourceNames().Where(s => s.EndsWith(name));");
                    result.AppendLine();
                    result.AppendLine("            if (files.Count() > 0)");
                    result.AppendLine("            {");
                    result.AppendLine("                using (var stream = current.GetManifestResourceStream(files.First()))");
                    result.AppendLine("                {");
                    result.AppendLine("                    if (stream != null)");
                    result.AppendLine("                    {");
                    if (config.IsCompressed)
                    {
                        result.AppendLine("                        var data = compressorMethod.Invoke(null, new object[] { stream }) as byte[];");
                        result.AppendLine("                        return Assembly.Load(data);");
                    }
                    else
                    {
                        result.AppendLine("                        var data = new byte[stream.Length];");
                        result.AppendLine("                        stream.Read(data, 0, data.Length);");
                        result.AppendLine("                        return Assembly.Load(data);");
                    }
                    result.AppendLine("                    }");
                    result.AppendLine("                }");
                    result.AppendLine("            }");
                }
                else
                {
                    result.AppendLine();
                    result.AppendLine("            var data = GCompressor.Decompress(AppDomain.CurrentDomain.BaseDirectory + name);");
                    result.AppendLine("            return Assembly.Load(data);");
                }
                if (config.IsEmbedded)
                {
                    result.AppendLine();
                    result.AppendLine("            return null;");
                }
                result.AppendLine("        }");
            }
            if (XAML.Length > 0)
            {
                result.AppendLine();
                result.AppendLine("        public string Decode(string value)");
                result.AppendLine("        {");
                result.AppendLine("            if (value != null && value.Length > 0)");
                result.AppendLine("            {");
                result.AppendLine("                return Encoding.UTF8.GetString(Convert.FromBase64String(value));");
                result.AppendLine("            }");
                result.AppendLine("            else");
                result.AppendLine("            {");
                result.AppendLine("                return string.Empty;");
                result.AppendLine("            }");
                result.AppendLine("        }");
                result.AppendLine();
                result.AppendLine("        public T FindControl<T>(DependencyObject parent, string value)");
                result.AppendLine("        {");
                result.AppendLine("            return (T)(object)LogicalTreeHelper.FindLogicalNode(parent, value);");
                result.AppendLine("        }");
            }
            result.AppendLine();
            result.AppendLine("        public void InitializeComponent()");
            result.AppendLine("        {");
            result.AppendLine("            Dispatcher.UnhandledException += (dS, dE) =>");
            result.AppendLine("            {");
            result.AppendLine("                dE.Handled = true;");
            result.AppendLine(@"                if(MessageBox.Show(dE.Exception.Message + ""\n프로그램을 계속 진행 하시겠습니까?"", ""런타임 오류"", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No)");
            result.AppendLine(@"                {");
            result.AppendLine(@"                    Application.Current.Shutdown();");
            result.AppendLine(@"                }");
            result.AppendLine("            };");
            result.AppendLine();
            result.AppendLine("            Dispatcher.UnhandledExceptionFilter += (dS, dE) =>");
            result.AppendLine("            {");
            result.AppendLine("                dE.RequestCatch = true;");
            result.AppendLine("            };");
            result.AppendLine();
            if (XAML.Length > 0)
            {
                result.AppendLine($@"            window = (XamlReader.Parse(Decode(""{_XAML}"")) as Window);");
                result.AppendLine("            window.Loaded += (s, e) => Initialize();");
                result.AppendLine("            window.Closing += (s, e) =>");
                result.AppendLine("            {");
                result.AppendLine("                if (Closing != null) Closing();");
                result.AppendLine("            };");
                result.AppendLine("            window.Show();");
            }
            else
            {
                result.AppendLine("            Initialize();");
                if (!config.IsService)
                {
                    result.AppendLine("            Application.Current.Shutdown();");
                }
            }
            result.AppendLine("        }");
            result.AppendLine();
            result.Append(ConvertAssistant.Indentation(source, 2));
            result.AppendLine("    }");
            result.AppendLine("}");

            return result.ToString();
        }

        private void CopyDirectory(string source, string destination, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(source);

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destination, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    CopyDirectory(subdir.FullName, Path.Combine(destination, subdir.Name), copySubDirs);
                }
            }
        }
        #endregion

        #region 사용자 함수
        /// <summary>
        /// 외부 참조를 추가합니다.
        /// </summary>
        /// <param name="path">외부 참조 파일의 경로입니다.</param>
        public void LoadReference(string path)
        {
            if (!IsNameContains(References, path))
            {
                // 참조 추가
                References.Add(path);
                LoadedReferences.Add(path);

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
        }

        /// <summary>
        /// 외부 참조를 비동기로 추가합니다.
        /// </summary>
        /// <param name="path">외부 참조 파일의 경로입니다.</param>
        public async Task LoadReferenceAsync(string path)
        {
            await Task.Run(() => LoadReference(path));
        }

        /// <summary>
        /// 실행시 필요한 추가 종속성을 추가합니다.
        /// </summary>
        /// <param name="path">추가 종속성 파일의 경로입니다.</param>
        public void LoadDependencies(string path)
        {
            if (!Dependencies.Contains(path))
            {
                Dependencies.Add(path);
            }
        }

        /// <summary>
        /// 실행시 필요한 추가 종속성을 비동기로 추가합니다.
        /// </summary>
        /// <param name="path">추가 종속성 파일의 경로입니다.</param>
        public async Task LoadDependenciesAsync(string path)
        {
            await Task.Run(() => LoadDependencies(path));
        }

        /// <summary>
        /// 소스를 빌드하여 컴파일된 파일을 생성합니다.
        /// </summary>
        /// <param name="path">컴파일된 파일을 생성할 경로입니다.</param>
        /// <param name="isExecutable">실행 파일 형태로 컴파일 할지 여부를 설정합니다.</param>
        public GCompilerResults Build(string path, GCompilerConfig config)
        {
            parameters.OutputAssembly = path;
            parameters.CompilerOptions = "/platform:x86 /target:winexe";
            parameters.EmbeddedResources.Clear();
            string fullSource = ConvertToFullSource(Source, config);

            foreach (string dll in References)
            {
                if (File.Exists(dll))
                {
                    string target = dll;
                    if (config.IsCompressed && Path.GetFileNameWithoutExtension(target) != "GSharp.Compressor")
                    {
                        var tempFileName = Path.GetTempFileName();
                        var targetCompressed = Path.GetTempPath() + $"{Path.GetFileNameWithoutExtension(target)}.pak";

                        if (!Path.GetTempPath().StartsWith(Path.GetDirectoryName(target)))
                        {
                            File.Copy(target, tempFileName, true);
                        }
                                                
                        GCompressor.Compress(tempFileName, targetCompressed);
                        target = targetCompressed;
                    }

                    if (config.IsEmbedded)
                    {
                        parameters.EmbeddedResources.Add(target);
                    }
                    else
                    {
                        var destination = $@"{Path.GetDirectoryName(path)}\{Path.GetFileName(target)}";
                        if (target != destination)
                        {
                            File.Copy(target, destination, true);
                        }
                    }
                }
            }

            foreach (string directory in Dependencies)
            {
                if (Directory.Exists(directory))
                {
                    CopyDirectory(directory, Path.GetDirectoryName(path), true);
                }
            }

            GCompilerResults results = new GCompilerResults
            {
                Source = fullSource,
                Results = provider.CompileAssemblyFromSource(parameters, fullSource)
            };

            return results;
        }

        /// <summary>
        /// 소스를 빌드하여 컴파일된 파일을 비동기로 생성합니다.
        /// </summary>
        /// <param name="path">컴파일된 파일을 생성할 경로입니다.</param>
        /// <param name="isExecutable">실행 파일 형태로 컴파일 할지 여부를 설정합니다.</param>
        public async Task<GCompilerResults> BuildAsync(string path, GCompilerConfig config)
        {
            return await Task.Run(() => Build(path, config));
        }
        #endregion
    }
}
