using GSharp.Base.Objects.Numbers;
using GSharp.Extension;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects.Lists;
using GSharp.Graphic.Objects.Logics;
using GSharp.Graphic.Objects.Numbers;
using GSharp.Graphic.Objects.Strings;
using GSharp.Graphic.Scopes;
using GSharp.Graphic.Statements;
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

namespace GSharpSample
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listBlock.Items.Add(new EventBlock(new GCommand("this", "Loaded", "프로그램이 시작될 때", typeof(void), GCommand.CommandType.Event)));
            listBlock.Items.Add(new EventBlock(new GCommand("this", "Closing", "프로그램이 종료될 때", typeof(void), GCommand.CommandType.Event)));

            listBlock.Items.Add(new IfBlock());
            listBlock.Items.Add(new IfElseBlock());
            listBlock.Items.Add(new SetBlock());

            listBlock.Items.Add(new GateBlock());
            listBlock.Items.Add(new CompareBlock());

            listBlock.Items.Add(new EmptyListBlock());
            listBlock.Items.Add(new ItemBlock());
            listBlock.Items.Add(new ListAddBlock());

            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.PLUS));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.MINUS));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.MULTIPLY));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.DIVISION));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.MODULO));

            listBlock.Items.Add(new StringCatBlock());
            listBlock.Items.Add(new LengthBlock());

            listBlock.Items.Add(new LoopInfinityBlock());
            listBlock.Items.Add(new LoopNBlock());

            foreach (BaseBlock block in listBlock.Items)
            {
                block.Margin = new Thickness(0, 0, 0, 10);
            }
        }
    }
}
