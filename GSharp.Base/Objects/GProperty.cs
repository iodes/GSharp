using GSharp.Base.Cores;
using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects
{
    public class GProperty : GObject
    {
        #region 속성
        public GCommand GCommand { get; set; }
        #endregion

        #region 생성자
        public GProperty(GCommand command)
        {
            GCommand = command;
        }
        #endregion

        public override string ToSource()
        {
            return GCommand.FullName;
        }
    }
}
