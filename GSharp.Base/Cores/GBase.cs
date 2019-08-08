using System;

namespace GSharp.Base.Cores
{
    [Serializable]
    public abstract class GBase
    {
        /// <summary>
        /// 객체를 네이티브 소스로 변환합니다.
        /// </summary>
        /// <returns></returns>
        public abstract string ToSource();
    }
}
