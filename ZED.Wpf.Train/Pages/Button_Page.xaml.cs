﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using ZED.CustomControl;

namespace ZED.Wpf.Train
{
    /// <summary>
    /// Button_Page.xaml 的交互逻辑
    /// </summary>
    public partial class Button_Page : Page
    {
        public Button_Page()
        {
            InitializeComponent();
        }

        private void btn_wait_Click(object sender, RoutedEventArgs e)
        {
            WaitingWinBox.Show(new Action(() => 
            {
                Thread.Sleep(2000);
            }));
        }

        private void btn_wait1_Click(object sender, RoutedEventArgs e)
        {
            var waitWin = new WaitingWinBox();
            waitWin.Owner = ComControlHelper.GetTopWindow();
            waitWin.Show();//此处必须用show

            Task.Factory.StartNew(new Action(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    waitWin.TipMessageAsync(string.Format("加载 {0} ...", i));
                    Thread.Sleep(1000);
                }
            })).ContinueWith(x =>
            {
                waitWin.CloseAsync();
            });
            
        }
        private void btn_winBase_Click(object sender, RoutedEventArgs e)
        {
            var winBase = new TestWinBase();
            winBase.Owner = ComControlHelper.GetTopWindow();
            winBase.Show();
        }

        private void btn_msg_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxEx.ShowInfo("我是提示信息！");
        }

        private void btn_war_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxEx.ShowWarn("我是警告信息！我是警告信息！我是警告信息！我是警告信息！我是警告信息！我是警告信息！我是警告信息！我是警告信息！我是警告信息！我是警告信息！");
        }

        private void btn_error_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxEx.ShowError("我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，我是错误，");
        }

        private void btn_question_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxEx.ShowQuestion("确认删除？");
            if (result)
            {
                MessageBox.Show("true");
            }
            else
            {
                MessageBox.Show("false");
            }
        }
    }
}
