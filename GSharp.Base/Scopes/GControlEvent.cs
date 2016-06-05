using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GControlEvent : GScope, IModule
    {
        public List<GStatement> Content = new List<GStatement>();

        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        public GControlEvent(GCommand command)
        {
            _GCommand = command;
        }

        public void Append(GStatement statement)
        {
            Content.Add(statement);
        }

        public void Clear()
        {
            Content.Clear();
        }

        public override string ToSource()
        {
            var source = new StringBuilder();

            source.AppendFormat("FindControl(window, \"{0}\").{1} += () =>\n", GCommand.NamespaceName, GCommand.MethodName);
            source.AppendLine("{");

            foreach (GStatement statement in Content)
            {
                source.AppendLine(ConvertAssistant.Indentation(statement.ToSource()));
            }

            source.AppendLine("};");

            return source.ToString();
        }
    }
}
