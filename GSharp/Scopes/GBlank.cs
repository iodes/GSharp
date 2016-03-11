using System.Text;
using System.Collections.Generic;
using GSharp.Cores;

namespace GSharp.Scopes
{
    public class GBlank : GScope
    {
        #region 객체
        private List<GBase> listObject = new List<GBase>();
        #endregion

        #region 사용자 함수
        public void Append(GBase valueObject)
        {
            listObject.Add(valueObject);
        }
        #endregion

        public override string ToSource()
        {
            StringBuilder builderCode = new StringBuilder();
            foreach (GBase target in listObject)
            {
                builderCode.AppendLine(target.ToSource());
            };
            return builderCode.ToString();
        }
    }
}
