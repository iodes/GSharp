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
using GSharp.Base.Objects.Strings;

namespace GSharp.Graphic.Objects.Strings
{
    /// <summary>
    /// StringPropertyBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringCatBlock : StringBlock
    {
        #region Holes
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;
        #endregion

        #region Objects
        public GStringCat GStringCat
        {
            get
            {
                GString obj1 = StringHole1.StringBlock?.GString;
                GString obj2 = StringHole2.StringBlock?.GString;

                return new GStringCat(obj1, obj2);
            }
        }

        public override GString GString
        {
            get
            {
                return GStringCat;
            }
        }

        public override GObject GObject
        {
            get
            {
                return GStringCat;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GObject };
        }
        #endregion

        // 생성자
        public StringCatBlock()
        {
            InitializeComponent();

            _HoleList = new List<BaseHole> { StringHole1, StringHole2 };

            InitializeBlock();
        }
    }
}
