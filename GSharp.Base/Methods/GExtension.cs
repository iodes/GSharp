using GSharp.Base.Cores;

namespace GSharp.Base.Methods
{
    public class GExtension : GMethod
    {
        #region 속성
        public string Name { get; set; }

        public string Method { get; set; }

        public string Namespace { get; set; }
        #endregion

        public override string ToSource()
        {
            return string.Format("{0}.{1}", Namespace, Method);
        }
    }
}
