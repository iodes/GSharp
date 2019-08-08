using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects
{
    public class GVariable : GSettableObject
    {
        public override Type SettableType
        {
            get
            {
                return typeof(object);
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
        }
        private string _Name;

        public GVariable(string name)
        {
            _Name = name;
        }

        public override string ToSource()
        {
            return Name;
        }
    }
}
