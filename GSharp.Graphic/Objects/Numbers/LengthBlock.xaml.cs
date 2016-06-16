using System.Collections.Generic;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Objects.Numbers;
using System;
using System.Xml;
using GSharp.Graphic.Controls;

namespace GSharp.Graphic.Objects.Numbers
{
    /// <summary>
    /// ComputeBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LengthBlock : NumberBlock
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
        public GLength GLength
        {
            get
            {
                GObject obj1 = StringHole1.StringBlock?.GObject;

                return new GLength(obj1);
            }
        }

        public override GNumber GNumber
        {
            get
            {
                return GLength;
            }
        }

        public override GObject GObject
        {
            get
            {
                return GLength;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GObject };
        }
        #endregion
        
        #region Constructor
        public LengthBlock()
        {
            // Initialize Component
            InitializeComponent();

            // Initialize Hole List
            _HoleList = new List<BaseHole> { StringHole1 };
            
            // Initialize Block
            InitializeBlock();
        }
        #endregion
        
        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            LengthBlock block = new LengthBlock();
            XmlNode node = element.SelectSingleNode("Holes/Hole/Block");
            block.StringHole1.StringBlock = LoadBlock(node as XmlElement, blockEditor) as ObjectBlock;

            return block;
        }
    }
}