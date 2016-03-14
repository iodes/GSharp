using GSharp.Base.Cores;

namespace GSharp.Base.Objects
{
    public class GVariable : GObject
    {
        #region 속성
        public string Name { get; set; }

        public VariableType CurrentType { get; set; }
        #endregion

        #region 열거형
        public enum VariableType
        {
            OBJECT,
            STRING,
            NUMBER
        }
        #endregion

        #region 생성자
        public GVariable(string valueName, VariableType valueVariableType = VariableType.OBJECT)
        {
            Name = valueName;
            CurrentType = valueVariableType;
        }
        #endregion

        #region 사용자 함수
        public string GetTypeString()
        {
            string typeString = string.Empty;
            switch (CurrentType)
            {
                case VariableType.OBJECT:
                    typeString = "object";
                    break;

                case VariableType.STRING:
                    typeString = "string";
                    break;

                case VariableType.NUMBER:
                    typeString = "long";
                    break;
            }

            return typeString;
        }
        #endregion

        public override string ToSource()
        {
            return Name;
        }
    }
}
