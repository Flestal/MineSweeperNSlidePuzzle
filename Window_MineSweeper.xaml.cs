﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MineSweeperNSlidePuzzle
{
    /// <summary>
    /// Window_MineSweeper.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window_MineSweeper : Window
    {
        //delegate void FHideWindow();
        Button[,] buttons;
        short[,] WhereMine;
        public int xx { get; set; }
        public int yy { get; set; }
        public int MineCount { get; set; }
        bool GameStart,GameOver;
        public Window_MineSweeper(int x,int y,int MineCnt)
        {
            InitializeComponent();
            xx = x; yy = y;MineCount = MineCnt;
            Width = 15 + 40 * xx + 30;
            Height = 128 + 40 * (yy+1) + 15;
            buttons = new Button[xx, yy];
            WhereMine=new short[xx, yy];
            for (int i=0; i < xx; i++)
            {
                for(int j=0; j < yy; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Name = "btn_" + i.ToString() + "_" + j.ToString();
                    buttons[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    buttons[i, j].VerticalAlignment = VerticalAlignment.Top;
                    buttons[i, j].Margin = new Thickness(15 + 40 * i, 128 + 40 * j, 0, 0);
                    buttons[i, j].Height = 35;
                    buttons[i, j].Width = 35;
                    buttons[i, j].Click += new RoutedEventHandler(btn_Click);
                    buttons[i, j].MouseLeave += new MouseEventHandler(btn_MouseLeave);
                    buttons[i, j].MouseDown += new MouseButtonEventHandler(btn_MouseDown);
                    buttons[i, j].MouseUp += new MouseButtonEventHandler(btn_MouseUp);
                    buttons[i, j].FontSize = 24;
                    buttons[i, j].Content = "";
                    Grid_.Children.Add(buttons[i, j]);
                }
            }
            GameStart = false;
            GameOver = false;
        }
        SolidColorBrush brush_reset = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        SolidColorBrush brush_found = new SolidColorBrush(Color.FromRgb(180, 180, 180));
        

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (GameOver) return;
            Button btn = (Button)sender;
            int[] xy = getWhatButton(btn);
            if (GameStart == false)
            {
                MakeNewBoard(xy[0], xy[1]);
            }
            if (btn.Content == "F")
            {
                return;
            }
            
            SearchIfZero(xy[0], xy[1]);
            
        }
        void SearchIfZero(int x,int y)
        {
            if (x >= xx || y >= yy || x < 0 || y < 0) { return; }
            Button btn = buttons[x, y];
            if (btn.Background == brush_found||btn.Content.ToString()=="F") return;
            if (WhereMine[x, y] == 1000)
            {
                btn.Content = "B";
                for(int i = 0; i < xx; i++)
                {
                    for(int j=0; j < yy; j++)
                    {
                        if (WhereMine[i, j] == 1000)
                        {
                            buttons[i,j].Content = "B";
                        }
                    }
                }
                img_Face.Source = new BitmapImage(new Uri("/MineSweeper3.png", UriKind.RelativeOrAbsolute));
                GameOver = true;
            }
            
            if (btn.Content.ToString() == "")
            {
                btn.Content = WhereMine[x, y];
                btn.Background = brush_found;
            }
            if (WhereMine[x, y] == 0)
            {
                for(int i = 0; i < 3; i++)
                {
                    for(int j=0;j < 3; j++)
                    {
                        SearchIfZero(x+i - 1,y+ j - 1);
                    }
                }
            }
            if (GameEndCheck())
            {
                img_Face.Source = new BitmapImage(new Uri("/MineSweeper2.png", UriKind.RelativeOrAbsolute));
                GameOver = true;
            }
        }
        int ClickFlag_X,ClickFlag_Y;
        private void btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed||e.MiddleButton==MouseButtonState.Pressed)
            {
                int[] xy = getWhatButton((Button)sender);
                ClickFlag_X = xy[0]; ClickFlag_Y = xy[1];
            }
            
        }
        private void btn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickFlag_X != -1 && ClickFlag_Y != -1)
            {
                if(e.ChangedButton==MouseButton.Right)
                {
                    if(buttons[ClickFlag_X, ClickFlag_Y].Content == "F")
                    {
                        buttons[ClickFlag_X, ClickFlag_Y].Content = "";
                    }
                    else if(buttons[ClickFlag_X, ClickFlag_Y].Content == "")
                    {
                        buttons[ClickFlag_X, ClickFlag_Y].Content = "F";
                    }
                }
                if (e.ChangedButton == MouseButton.Middle)
                {
                    if (GameStart == false)
                    {
                        MakeNewBoard(ClickFlag_X, ClickFlag_Y);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            SearchIfZero(ClickFlag_X + i - 1, ClickFlag_Y + j - 1);
                        }
                    }
                }
            }
        }
        private void btn_MouseLeave(object sender, MouseEventArgs e)
        {
            ClickFlag_X = -1;ClickFlag_Y = -1;
        }
        
        void MakeNewBoard(int x, int y)
        {
            GameStart = true;
            Random R = new Random(DateTime.Now.Millisecond);
            for(int i=0;i<MineCount;i++)
            {
                int x_ = -1, y_ = -1;
                do
                {
                    x_ = R.Next(xx);
                    y_ = R.Next(yy);
                }while((x_==x && y_ == y) || WhereMine[x_,y_]==1000);
                WhereMine[x_, y_] = 1000;
            }
            for(int i = 0; i < xx; i++)
            {
                for(int j=0;j < yy; j++)
                {
                    if (WhereMine[i, j] == 1000) continue;
                    if (i - 1 >= 0 && j - 1 >= 0 && WhereMine[i - 1, j - 1] == 1000) WhereMine[i, j]++;
                    if (i - 1 >= 0 && WhereMine[i - 1, j] == 1000) WhereMine[i, j]++;
                    if (i - 1 >= 0 && j + 1 < yy && WhereMine[i - 1, j + 1] == 1000) WhereMine[i, j]++;
                    if (j - 1 >= 0 && WhereMine[i, j - 1] == 1000) WhereMine[i, j]++;
                    if (j + 1 < yy && WhereMine[i, j + 1] == 1000) WhereMine[i, j]++;
                    if (i + 1 < xx && j - 1 >= 0 && WhereMine[i + 1, j - 1] == 1000) WhereMine[i, j]++;
                    if (i + 1 < xx &&  WhereMine[i + 1, j] == 1000) WhereMine[i, j]++;
                    if (i + 1 < xx && j + 1 < yy && WhereMine[i + 1, j + 1] == 1000) WhereMine[i, j]++;
                }
            }
        }
        int[] getWhatButton(Button btn)
        {
            int[] xy=new int[2];
            string[] split=btn.Name.Split('_');
            xy[0] = int.Parse(split[1]);
            xy[1] = int.Parse(split[2]);
            return xy;
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            img_Face.Source = new BitmapImage(new Uri("/MineSweeper1.png",UriKind.RelativeOrAbsolute));
            GameStart = false;
            GameOver = false;
            foreach(Button btn in buttons)
            {
                btn.Content = "";
                btn.Background = brush_reset;
            }
            WhereMine = new short[xx, yy];
        }
        bool GameEndCheck()
        {
            int cnt = 0;
            for(int i = 0; i < xx; i++)
            {
                for(int j=0;j<yy; j++)
                {
                    if (WhereMine[i, j] != 1000 && buttons[i, j].Content != "")
                    {//폭탄이 아니면서 그 위치 버튼을 깠으면 cnt 올라감
                        cnt++;
                    }
                }
            }
            return cnt==(xx*yy-MineCount)?true:false;
        }
    }
}
