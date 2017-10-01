namespace GSharp.Support.Utilities
{
    public static class StringExtension
    {
        public static bool StringEquals(this string val1, string val2)
        {
            return val1.ToLower() == val2.ToLower();
        }

        public static bool StringEquals(this object val1, string val2)
        {
            return val1.ToString().ToLower() == val2.ToLower();
        }

        public static bool StringContains(this string val1, string val2)
        {
            return val1.ToLower().Contains(val2.ToLower());
        }

        public static bool StringContains(this object val1, string val2)
        {
            return val1.ToString().ToLower().Contains(val2.ToLower());
        }
    }
}
