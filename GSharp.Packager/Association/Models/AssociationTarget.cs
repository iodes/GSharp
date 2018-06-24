using System.Collections.Generic;

namespace GSharp.Packager.Association
{
    public class AssociationTarget
    {
        /// <summary>
        /// 연결 확장자 컬렉션을 가져옵니다.
        /// </summary>
        public List<AssociationType> Associations { get; } = new List<AssociationType>();

        /// <summary>
        /// 앱의 표시 이름을 가져오거나 설정합니다.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 앱의 제조사 이름을 가져오거나 설정합니다.
        /// </summary>
        public string CompanyName { get; set; }
    }
}
