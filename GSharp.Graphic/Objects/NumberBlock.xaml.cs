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
using GSharp.Graphic.Core;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// NumberBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumberBlock : ObjectBlock
    {
        private long number;
        public long Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                NumberText.Text = value.ToString();
            }
        }

        public GNumber GNumber
        {
            get
            {
                long number = long.Parse(NumberText.Text);
                return new GNumber(number);
            }
        }

        public override GObject GObject
        {
            get
            {
                return GNumber;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GObject };
            }
        }

        public NumberBlock()
        {
            InitializeComponent();
        }

        private void NumberText_TextChanged(object sender, TextChangedEventArgs e)
        {
            long number;

            if (long.TryParse(NumberText.Text, out number))
            {
                Number = number;
            }
            else
            {
                Number = Number;
            }
        }
    }
}
