using System.Collections.Generic;

namespace GSharp.Extension
{
    public class GModule
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Summary { get; set; }

        public string Package { get; set; }

        public List<KeyValuePair<string, string>> Commands { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
