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
        public IVariable Variable { get; set; }

        public GDefine(IVariable variable)
        {
            Variable = variable;
        }

        public override string ToSource()
        {
            var compiler = new CSharpCodeProvider();
            var type = new CodeTypeReference(Variable.VariableType);
            
            return string.Format("public {0} {1};", compiler.GetTypeOutput(type), Variable.Name);
        }
    }
}
