using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;
using GSharp.Extension.Exports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Customs
{
    public class GControlProperty : GSettableObject, ICustom
    {
        public GExport GExport
        {
            get
            {
                return _GExport;
            }
        }
        private GExport _GExport;

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

        public Type CustomType
        {
            get
            {
                return GExport.ObjectType;
            }
        }

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
