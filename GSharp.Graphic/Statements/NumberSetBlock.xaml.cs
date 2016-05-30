using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Statements;
using GSharp.Base.Objects.Numbers;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// NumberSetBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumberSetBlock : PrevStatementBlock
    {
        public NumberSetBlock()
        {
            InitializeComponent();
            InitializeBlock();
        }

        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }

        public override StatementBlock GetLastBlock()
        {
            if (NextConnectHole.StatementBlock == null)
            {
                return this;
            }

            return NextConnectHole.StatementBlock.GetLastBlock();
        }

        public GSet<GNumberVariable, GNumber> GSet
        {
            get
            {
                string variableName = VariableSelect.Text;
                GNumberVariable variable = new GNumberVariable(variableName);

                GNumber number = NumberHole?.NumberBlock?.GNumber;

                if (number == null)
                {
                    throw new ToObjectException("변수 설정 블럭이 완성되지 않았습니다.", this);
                }

                return new GSet<GNumberVariable, GNumber>(variable, number);
            }
        }

        public override GStatement GStatement
        {
            get
            {
                return GSet;
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { NumberHole, NextConnectHole };
            }
        }

        public override List<GBase> ToGObjectList()
        {
            List<GBase> baseList = new List<GBase>();
            baseList.Add(GSet);

            List<GBase> nextList = NextConnectHole?.StatementBlock?.ToGObjectList();
            if (nextList != null)
            {
                baseList.AddRange(nextList);
            }

            return baseList;
        }
    }
}
