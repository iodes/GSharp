using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// PrevConnectHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PrevConnectHole : INotifyPropertyChanged
    {
        public Brush BodyColor
        {
            get
            {
                return _BodyColor;
            }
            set
            {
                _BodyColor = value;
                OnPropertyChanged();
            }
        }
        private Brush _BodyColor = Brushes.White;

        public Brush BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
                OnPropertyChanged();
            }
        }
        private Brush _BorderColor = Brushes.DarkOrange;


        public event PropertyChangedEventHandler PropertyChanged;

        public PrevConnectHole()
        {
            InitializeComponent();
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
