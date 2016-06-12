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

        public string ControlName
        {
            get
            {
                return _ControlName;
            }
        }
        private string _ControlName;

        public override Type SettableType
        {
            get
            {
                return GExport.ObjectType;
            }
        }

        private GExport _GExport;

        public GControlProperty(string controlName, GExport export)
        {
            _ControlName = controlName;
            _GExport = export;

            if (export.ObjectType == typeof(void))
            {
                throw new Exception("Property가 아닙니다.");
            }
        }

        public override string ToSource()
        {
            return string.Format($@"FindControl<{GExport.NamespaceName}>(window, ""{ControlName}"").{GExport.MethodName}");
        }
    }
}
