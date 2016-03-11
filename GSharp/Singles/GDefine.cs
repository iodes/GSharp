using GSharp.Cores;
using GSharp.Objects;

namespace GSharp.Singles
{
    public class GDefine : GSingle
    {
        public string Name { get; set; }

        public GDefine(string valueName)
        {
            Name = valueName;
        }

        public override string ToSource()
        {
            return string.Format
                (
                    "public object {0};",
                    Name
                );
        }

        public GVariable GetVariable()
        {
            return new GVariable(Name);
        }
    }
}
