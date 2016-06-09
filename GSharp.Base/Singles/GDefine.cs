using System;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using Microsoft.CSharp;
using System.CodeDom;

namespace GSharp.Base.Singles
{
    [Serializable]
    public class GDefine : GSingle
    {
        public GVariable Variable { get; set; }

        public GDefine(GVariable variable)
        {
            Variable = variable;
        }

        public override string ToSource()
        {
            var compiler = new CSharpCodeProvider();
            
            return string.Format("public GObject {0} = new GString();", Variable.Name);
        }
    }
}
