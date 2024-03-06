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
        //delegate void FHideWindow();
        Button[,] buttons;
        int[,] Board;
        int[] ZeroPos=new int[2];
        public int xx { get; set; }
        public int yy { get; set; }
        bool GameStart, GameOver;
        int temp;
        public Window_SlidePuzzle(int x, int y)
        {
            InitializeComponent();
            xx = x; yy = y;
            buttons = new Button[xx, yy];
            Board = new int[xx, yy];
            Width = 15 + 65 * xx + 30;
            Height = 128 + 65 * (yy + 1) + 15;
            for (int i = 0; i < xx; i++)
            {
                for (int j = 0; j < yy; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Name = "btn_" + i.ToString() + "_" + j.ToString();
                    buttons[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    buttons[i, j].VerticalAlignment = VerticalAlignment.Top;
                    buttons[i, j].Margin = new Thickness(15 + 65 * i, 128 + 65 * j, 0, 0);
                    buttons[i, j].Height = 60;
                    buttons[i, j].Width = 60;
                    buttons[i, j].Click += new RoutedEventHandler(btn_Click);
                    buttons[i, j].FontSize = 24;
                    Board[i, j] = i*yy+j+1;
                    Grid_.Children.Add(buttons[i, j]);
                }
            }
            Board[xx - 1, yy - 1] = 0;
            ZeroPos[0]=xx-1; ZeroPos[1]=yy-1;
            GameStart = false;
            GameOver = false;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (!GameStart)
            {
                StartShuffle(1000);
                return;
            }
            if (GameOver)
            {
                return;
            }
            Button btn = (Button)sender;
            int[] xy = getWhatButton(btn);
            if (xy != null && Board[xy[0], xy[1]]!=0)
            {
                ClickPosition(xy[0], xy[1]);
                
            }
        }
        int[] getWhatButton(Button btn)
        {
            int[] xy = new int[2];
            string[] split = btn.Name.Split('_');
            xy[0] = int.Parse(split[1]);
            xy[1] = int.Parse(split[2]);
            return xy;
        }
        void ClickPosition(int x, int y, bool render = true)
        {
            tb_Log.Text = "x : " + x + ", y : " + y+"\nZero x : "+ZeroPos[0]+", y : "+ZeroPos[1];
            if (x + 1 < xx && Board[x + 1, y] == 0)
            {
                temp = Board[x, y];
                Board[x, y] = Board[x + 1, y];
                Board[x + 1, y] = temp;
                ZeroPos[0] = x; ZeroPos[1] = y;
            }
            else if (x - 1 >= 0 && Board[x - 1, y] == 0)
            {
                temp = Board[x, y];
                Board[x, y] = Board[x - 1, y];
                Board[x - 1, y] = temp;
                ZeroPos[0] = x; ZeroPos[1] = y;
            }
            else if (y + 1 < yy && Board[x, y + 1] == 0)
            {
                temp = Board[x, y];
                Board[x, y] = Board[x, y + 1];
                Board[x, y + 1] = temp;
                ZeroPos[0] = x; ZeroPos[1] = y;
            }
            else if (y - 1 >= 0 && Board[x, y-1] == 0)
            {
                temp = Board[x, y];
                Board[x, y] = Board[x, y - 1];
                Board[x, y - 1] = temp;
                ZeroPos[0] = x; ZeroPos[1] = y;
            }
            if (render)
            {
                ButtonRender();
                if (EndCheck())
                {
                    tb_Log.Text = "Game End!";
                    GameOver = true;
                }
            }
        }
        void ButtonRender()
        {
            for(int i = 0; i < xx; i++)
            {
                for(int j=0; j < yy; j++)
                {
                    buttons[i,j].Content = Board[i,j].ToString();
                }
            }
        }
        void StartShuffle(int loop)
        {
            Random R = new Random(DateTime.Now.Millisecond);
            int a, b, c;
            for(int i = 0; i < loop; i++)
            {
                do
                {
                    a = R.Next(2);
                    b = R.Next(2);
                } while (!ValidRandomCheck(a*2-1, b*2-1));
                c=R.Next(2);
                if (c == 0)
                {
                    ClickPosition(ZeroPos[0] + (a * 2 - 1), ZeroPos[1], false);
                }
                else
                {
                    ClickPosition(ZeroPos[0], ZeroPos[1] + (b * 2 - 1), false);
                }
                
            }
            ButtonRender();
            tb_Log.Text = "Game Start!";
            GameStart = true;
        }
        bool ValidRandomCheck(int x,int y)
        {
            bool res = true;
            if (ZeroPos[0] + x < 0)
            {
                res = false;
            }
            if (ZeroPos[0] + x >= xx)
            {
                res = false;
            }
            if (ZeroPos[1] + y < 0)
            {
                res = false;
            }
            if (ZeroPos[1] + y >= yy)
            {
                res = false;
            }
            return res;
        }
        bool EndCheck()
        {
            bool res = true;
            for(int i = 0; i < xx*yy-1; i++)
            {
                if (Board[i % xx, i / xx] != i + 1) res = false;
            }
            if (Board[xx-1,yy-1]!=0)res=false;
            return res;
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            GameOver = false;
            GameStart = false;
            tb_Log.Text = "Game Reset!";
        }
    }
}
