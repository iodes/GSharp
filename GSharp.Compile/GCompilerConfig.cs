namespace GSharp.Compile
{
    public class GCompilerConfig
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public string Product { get; set; }

        public string Copyright { get; set; }

        public string Trademark { get; set; }

        public string Version { get; set; }

        public string FileVersion { get; set; }

        public bool IsEmbedded { get; set; } = false;

        public bool IsCompressed { get; set; } = false;

        public bool IsService { get; set; } = false;
    }
}
