using GSharp.Base.Cores;
using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Customs
{
    [Serializable]
    public class GEnum : GCustom, IModule
    {
        #region 속성
        public override Type CustomType
        {
            get
            {
                return GCommand.ObjectType;
            }
        }

        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        public int SelectedIndex {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = (value >= GCommand.Optionals.Length) ? 0 : value;
            }
        }
        private int _SelectedIndex;
        #endregion

        #region 생성자
        private GEnum(GCommand command)
        {
            if (command.MethodType != GCommand.CommandType.Enum)
            {
                throw new Exception("Cannot create GEnum");
            }

            _GCommand = command;
        }

        public GEnum(GCommand command, int index = 0)
            : this(command)
        {
            SelectedIndex = index;
        }
        #endregion

        public override string ToCustomSource()
        {
            return GCommand.Optionals[SelectedIndex].FullName;
        }
    }
}
