using System;

namespace GSharp.Extension.Exceptions
{
    public class ModuleTypeException : Exception
    {
        public ModuleTypeException()
        {

        }

        public ModuleTypeException(string message) : base(message)
        {

        }

        public ModuleTypeException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
