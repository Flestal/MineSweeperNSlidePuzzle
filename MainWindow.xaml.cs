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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_SlidePuzzle_Click(object sender, RoutedEventArgs e)
        {
            int xx = int.Parse(tb_slide_XX.Text);
            int yy = int.Parse(tb_slide_YY.Text);
            Window_SlidePuzzle win_slide = new Window_SlidePuzzle(xx,yy);
            win_slide.Show();
        }

        private void btn_MineSweeper_Click(object sender, RoutedEventArgs e)
        {
            int xx=int.Parse(tb_mine_XX.Text);
            int yy = int.Parse(tb_mine_YY.Text);
            int mines = int.Parse(tb_mine_Mines.Text);
            Window_MineSweeper win_Mine = new Window_MineSweeper(xx,yy,mines);
            win_Mine.Show();
        }
    }
}
