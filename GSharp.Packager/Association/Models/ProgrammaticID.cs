namespace GSharp.Packager.Association
{
    public class ProgrammaticID
    {
        /// <summary>
        /// 확장자 아이콘을 가져오거나 설정합니다.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 확장자의 설명을 가져오거나 설정합니다.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 마우스 오버시 표시될 정보를 가져오거나 설정합니다.
        /// 설정되지 않으면 확장자의 설명을 대신 사용합니다.
        /// </summary>
        public string InfoTip { get; set; }

        /// <summary>
        /// 확장자 커멘드를 갸져오거나 설정합니다.
        /// </summary>
        public ShellCommand Command { get; set; }

        /// <summary>
        /// 파일 타입을 가져오거나 설정합니다.
        /// </summary>
        public FileType Type { get; set; }
    }
}
