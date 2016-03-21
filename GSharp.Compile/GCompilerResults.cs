using System.CodeDom.Compiler;

namespace GSharp.Compile
{
    public class GCompilerResults
    {
        public string Source { get; set; }

        public CompilerResults Results
        {
            get
            {
                return _Results;
            }
            set
            {
                _Results = value;
                _IsSuccess = Results.Errors.Count <= 0;
            }
        }

        private CompilerResults _Results;

        public bool IsSuccess
        {
            get
            {
                return _IsSuccess;
            }
        }
        private bool _IsSuccess = false;
    }
}
