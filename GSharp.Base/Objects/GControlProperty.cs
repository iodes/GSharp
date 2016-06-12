using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;
using GSharp.Extension.Exports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects
{
    public class GControlProperty : GSettableObject
    {
        public GExport GExport
        {
            get
            {
                return _GExport;
            }
        }

        public override Type SettableType
        {
            get
            {
                return GExport.ObjectType;
            }
        }

        private GExport _GExport;

        public GControlProperty(GExport export)
        {
            _GExport = export;

            if (export.ObjectType == typeof(void))
            {
                throw new Exception("Property가 아닙니다.");
            }
        }

        public override string ToSource()
        {
            return string.Format("FindControl(window, {0}).{1}", GExport.NamespaceName, GExport.MethodName);
        }
    }
}
