using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MineSweeperNSlidePuzzle
{
    /// <summary>
    /// Window_SlidePuzzle.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window_SlidePuzzle : Window
    {
        delegate void FHideWindow();
        public Window_SlidePuzzle()
        {
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            //base.OnClosing(e);
            e.Cancel = true;
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new FHideWindow(_HideThisWindow));
        }
        void _HideThisWindow()
        {
            this.Hide();
        }
    }
}
