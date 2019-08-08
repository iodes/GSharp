using System;
using GSharp.Common.Extensions;
using GSharp.Common.Objects;

namespace GSharp.Base.Objects.Customs
{
    public class GControlProperty : GSettableObject, ICustomObject
    {
        public string ControlName { get; }
        
        public IGExportedData ExportedData { get; }

        public Type CustomType => ExportedData.ObjectType;
        
        public override Type SettableType => ExportedData.ObjectType;

        public GControlProperty(string controlName, IGExportedData exportedData)
        {
            ControlName = controlName;
            ExportedData = exportedData;
            
            if (ExportedData.ObjectType == typeof(void))
            {
                throw new Exception("Its value is not Property.");
            }
        }

        public override string ToSource()
        {
            return $@"FindControl<{ExportedData.NamespaceName}>(window, ""{ControlName}"").{ExportedData.MethodName}";
        }
    }
}
