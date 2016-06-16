using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Objects.Strings;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GSharp.Graphic.Objects.Strings
{
    public class StringConstBlock : StringBlock
    {
        public string String
        {
            get
            {
                return _GStringConst.String;
            }
            set
            {
                _GStringConst.String = value;
            }
        }

        public override GString GString
        {
            get
            {
                return _GStringConst;
            }
        }

        public GStringConst GStringConst
        {
            get
            {
                return _GStringConst;
            }
        }
        private GStringConst _GStringConst;

        public override GObject GObject
        {
            get
            {
                return GString;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GObject };
        }

        public StringConstBlock() : this("")
        {
        }

        public StringConstBlock(string constString)
        {
            _GStringConst = new GStringConst(constString);
        }

        protected override void SaveBlockAttribute(XmlWriter writer)
        {
            writer.WriteAttributeString("String", String);
        }

        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            var constString = element.GetAttribute("String");

            return new StringConstBlock(constString);
        }
    }
}
