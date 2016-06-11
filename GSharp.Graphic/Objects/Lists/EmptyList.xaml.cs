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
using GSharp.Extension;
using GSharp.Base.Objects.Customs;
using GSharp.Base.Objects.Lists;

namespace GSharp.Graphic.Objects.Lists
{
    /// <summary>
    /// EmptyList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EmptyList : ListBlock
    {
        public override GList GList
        {
            get
            {
                return GEmptyList;
            }
        }

        public GEmptyList GEmptyList
        {
            get
            {
                return _GEmptyList;
            }
        }
        private GEmptyList _GEmptyList;


        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }
        private List<GBase> _GObjectList;

        // 생성자
        public EmptyList()
        {
            InitializeComponent();
            _GEmptyList = new GEmptyList();
            _GObjectList = new List<GBase>() { GObject };

            InitializeBlock();
        }
    }
}
