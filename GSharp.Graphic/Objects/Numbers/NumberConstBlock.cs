using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Objects.Numbers;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GSharp.Graphic.Objects.Numbers
{
    public class NumberConstBlock : NumberBlock
    {
        #region Objects
        // Number
        public double Number
        {
            get
            {
                return GNumberConst.Number;
            }
            set
            {
                GNumberConst.Number = value;
            }
        }
        public override GNumber GNumber
        {
            get
            {
                return GNumberConst;
            }
        }

        // GNumber
        public GNumberConst GNumberConst
        {
            get
            {
                return _GNumberConst;
            }
        }
        private GNumberConst _GNumberConst;

        // GObject
        public override GObject GObject
        {
            get
            {
                return GNumber;
            }
        }

        // GObjectList
        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }
        private List<GBase> _GObjectList;
        #endregion

        // Constructor
        public NumberConstBlock() : this(0)
        {
        }

        public NumberConstBlock(double numberConst)
        {
            // Initialize Objects
            _GNumberConst = new GNumberConst(numberConst);
            _GObjectList = new List<GBase> { GNumber };
        }

        protected override void SaveBlockAttribute(XmlWriter writer)
        {
            writer.WriteAttributeString("Number", Number.ToString());
        }

        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            double number;
            var constNumber = element.GetAttribute("Number");

            if (!double.TryParse(constNumber, out number))
            {
                number = 0;
            }

            return new NumberConstBlock(number);
        }
    }
}
