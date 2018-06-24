namespace GSharp.Packager.Association
{
    public class AssociationType
    {
        /// <summary>
        /// 확장자 아이콘을 가져오거나 설정합니다.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 파일 확장자를 가져오거나 설정합니다.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 정보에 표시될 정보를 가져오거나 설정합니다.
        /// </summary>
        public string InfoTip { get; set; }

        /// <summary>
        /// 확장자 커멘드를 갸져오거나 설정합니다.
        /// </summary>
        public ShellCommand Command { get; set; }
    }
}
