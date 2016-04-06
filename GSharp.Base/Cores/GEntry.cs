using GSharp.Base.Scopes;
using GSharp.Base.Singles;
using GSharp.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Cores
{
    public class GEntry : GBase
    {
        private List<GDefine> defineList = new List<GDefine>();
        private List<GEvent> eventList = new List<GEvent>();
        private List<GFunction> functionList = new List<GFunction>();

        public void Append(GEvent evt)
        {
            eventList.Add(evt);
        }

        public void Append(GDefine def)
        {
            defineList.Add(def);
        }

        public void Append(GFunction func)
        {
            functionList.Add(func);
        }
        
        public override string ToSource()
        {
            StringBuilder source = new StringBuilder();

            foreach (GDefine def in defineList)
            {
                source.AppendLine(def.ToSource());
            }

            source.AppendLine("\npublic delegate void LoadedHandler();");
            source.AppendLine("public event LoadedHandler Loaded;\n");

            source.AppendLine("public void Initialize()");
            source.AppendLine("{");

            foreach (GEvent evt in eventList)
            {
                source.AppendLine(ConvertAssistant.Indentation(evt.ToSource()));
            }

            source.AppendLine(ConvertAssistant.Indentation("if (Loaded != null) Loaded();").TrimEnd());
            source.AppendLine("}");

            foreach (GFunction func in functionList)
            {
                source.AppendLine(func.ToSource());
            }

            return source.ToString();
        }
    }
}
