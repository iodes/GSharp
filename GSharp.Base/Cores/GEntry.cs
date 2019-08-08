using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GSharp.Base.Scopes;
using GSharp.Base.Singles;
using GSharp.Base.Utilities;

namespace GSharp.Base.Cores
{
    [Serializable]
    public class GEntry : GBase
    {
        private readonly List<GScope> scopes = new List<GScope>();
        private readonly List<GDefine> defines = new List<GDefine>();
        private readonly List<GFunction> functions = new List<GFunction>();

        public void Append(GScope scope)
        {
            scopes.Add(scope);
        }

        public void Append(GDefine define)
        {
            defines.Add(define);
        }

        public void Append(GFunction function)
        {
            functions.Add(function);
        }
        
        public override string ToSource()
        {
            var source = new StringBuilder();
            defines.ForEach(x => source.AppendLine(x.ToSource()));
            
            source.AppendLine();
            source.AppendLine("public delegate void LoadedHandler();");
            source.AppendLine("public event LoadedHandler Loaded;");
            
            source.AppendLine();
            source.AppendLine("public delegate void ClosingHandler();");
            source.AppendLine("public event ClosingHandler Closing;");
            
            source.AppendLine();
            source.AppendLine("public void Initialize()");
            source.AppendLine("{");

            scopes.ForEach(x => source.AppendLine(ConvertAssistant.Indentation(x.ToSource())));
            source.AppendLine(ConvertAssistant.Indentation("if (Loaded != null) Loaded();").TrimEnd());
            source.AppendLine("}");

            functions.ForEach(x => source.AppendLine(x.ToSource()));
            return source.ToString();
        }
    }
}
