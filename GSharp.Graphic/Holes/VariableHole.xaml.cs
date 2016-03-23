using GSharp.Graphic.Core;
using GSharp.Graphic.Objects;
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
using GSharp.Base.Cores;

namespace GSharp.Graphic.Holes
{
    /// <summary>
    /// VariableHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VariableHole : BaseObjectHole
    {
        public override BaseBlock Block
        {
            get
            {
                return VariableBlock;
            }
        }

        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return VariableBlock;
            }
        }

        public VariableBlock VariableBlock
        {
            get
            {
                return new VariableBlock(Variable.Text);
            }
        }

        public VariableHole()
        {
            InitializeComponent();
        }

        public void SetItemList(List<string> variableList)
        {
            Variable.Items.Clear();

            foreach (string variableName in variableList)
            {
                Variable.Items.Add(variableName);
            }
        }

        public void AddItem(string variableName)
        {
            Variable.Items.Add(variableName);
        }

        public void RemoveItem(string variableName)
        {
            Variable.Items.Remove(variableName);
        }
    }
}
