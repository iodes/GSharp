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
    /// NumberConstBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumberConstBlock : NumberBlock
    {
        #region Objects
        // Number
        public double Number
        {
            get
            {
                return _Number;
            }
            set
            {
                _Number = value;
                GNumber.Number = value;
            }
        }
        private double _Number = .0;

        // GNumber
        public GNumber GNumber
        {
            get
            {
                return _GNumber;
            }
        }
        private GNumber _GNumber;

        // GObject
        public override GObject GObject
        {
            get
            {
                return GNumber;
            }
        }

        // GObjectList
        public override List<GBase> GObjectList
        {
            get
            {
                return _GObjectList;
            }
        }
        private List<GBase> _GObjectList;
        #endregion

        // Constructor
        public NumberConstBlock()
        {
            // Initialize Objects
            _GNumber = new GNumber(_Number);
            _GObjectList = new List<GBase> { GNumber };

            InitializeComponent();
            InitializeBlock();
        }

        // NumberText TextChanged Event
        private void NumberText_TextChanged(object sender, TextChangedEventArgs e)
        {
            double number;
            if (double.TryParse(NumberText.Text, out number))
            {
                Number = number;
            }
            else
            {
                // No!
            }
        }
    }
}
