using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GBlank : GScope
    {
        private readonly List<GBase> listObject = new List<GBase>();

        public void Append(GBase valueObject)
        {
            listObject.Add(valueObject);
        }

        public override string ToSource()
        {
            var builderCode = new StringBuilder();
            foreach (GBase target in listObject)
            {
                builderCode.AppendLine(target.ToSource());
            };

            return builderCode.ToString();
        }
    }
}
