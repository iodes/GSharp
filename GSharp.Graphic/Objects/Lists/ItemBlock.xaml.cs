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
    public partial class ItemBlock : ObjectBlock
    {
        public GItem GItem
        {
            get
            {
                var list = ListHole.ObjectBlock?.GObject;
                var index = NumberHole.NumberBlock?.GObject;
                return new GItem(list, index);
            }
        }
        
        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }
        private List<GBase> _GObjectList;

        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }

        public override GObject GObject
        {
            get
            {
                return GItem;
            }
        }

        private List<BaseHole> _HoleList;

        // 생성자
        public ItemBlock()
        {
            InitializeComponent();

            _HoleList = new List<BaseHole>() { ListHole, NumberHole };
            _GObjectList = new List<GBase>() { GObject };

            InitializeBlock();
        }
    }
}
