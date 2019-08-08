namespace GSharp.Base.Objects.Lists
{
    public class GEmptyList : GList
    {
        public override string ToSource()
        {
            return "new List<object>()";
        }
    }
}
