using GSharp.Base.Cores;

namespace GSharp.Base.Methods
{
    public class GPrint : GMethod
    {
        public override string ToSource()
        {
            return "Console.WriteLine";
        }
    }
}
