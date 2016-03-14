﻿using GSharp.Base.Cores;
using GSharp.Graphic.Core;
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

namespace GSharp.Graphic.Holes
{
    /// <summary>
    /// LogicHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogicHole : BaseHole
    {
        public GLogic Logic;

        public LogicBlock LogicBlock
        {
            get
            {
                return (LogicBlock)RealLogicBlock.Child;
            }
            set
            {
                if (value == RealLogicBlock.Child) return;

                if (RealLogicBlock.Child != null)
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                RealLogicBlock.Child = value;
            }
        }

        public LogicHole()
        {
            InitializeComponent();
        }

        private void RealLogicBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine(e.NewSize);
        }
    }
}