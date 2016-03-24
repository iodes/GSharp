using System.Collections.Generic;

namespace GSharp.Extension
{
    public class GModule
    {
        public string Path { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Summary { get; set; }

        public string Namespace { get; set; }

        public List<GCommand> Commands { get; set; } = new List<GCommand>();
    }
}
