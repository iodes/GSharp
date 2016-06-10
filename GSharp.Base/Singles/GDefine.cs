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
        public GVariable Variable { get; }

        public GDefine(GVariable variable)
        {
            Variable = variable;
        }

        public override string ToSource()
        {
            return string.Format("public object {0} = \"\";", Variable.Name);
        }
    }
}
