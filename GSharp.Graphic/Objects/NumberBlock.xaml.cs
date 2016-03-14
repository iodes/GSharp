﻿using System;
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
        public NumberBlock()
        {
            InitializeComponent();
        }

        public override List<BaseHole> GetHoleList()
        {
            return new List<BaseHole>();
        }

        public override List<GBase> ToObject()
        {
            List<GBase> baseList = new List<GBase>();

            long number = long.Parse(NumberValue.Text);
            baseList.Add(new GNumber(number));

            return baseList;
        }
    }
}