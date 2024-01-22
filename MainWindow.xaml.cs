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

namespace MineSweeperNSlidePuzzle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window_MineSweeper win_Mine;
        Window_SlidePuzzle win_slide;
        public MainWindow()
        {
            InitializeComponent();
            win_Mine = new Window_MineSweeper(10,10,20);
            win_slide = new Window_SlidePuzzle();
        }

        private void btn_SlidePuzzle_Click(object sender, RoutedEventArgs e)
        {
            win_slide.Show();
        }

        private void btn_MineSweeper_Click(object sender, RoutedEventArgs e)
        {
            win_Mine.xx = 10;
            win_Mine.yy = 10;
            win_Mine.Show();
        }
    }
}
