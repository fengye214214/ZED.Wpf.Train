using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZED.IVMS7200;
using ZED.IVMS7200.IVMS7200;

namespace ZED.Wpf.Train
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 命令定义

        #region 菜单打开命令
        public RoutedCommand MenuButtonCommand
        {
            get { return (RoutedCommand)GetValue(MenuButtonCommandProperty); }
            set { SetValue(MenuButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuButtonCommandProperty =
            DependencyProperty.Register("MenuButtonCommand", typeof(RoutedCommand), typeof(Window), new PropertyMetadata(null));
        #endregion


        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.MenuButtonCommand = new RoutedCommand();
            var btn = new CommandBinding(this.MenuButtonCommand);
            btn.Executed +=btn_Executed;
            this.CommandBindings.Add(btn);
        }

        void btn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var btn = e.Source as Button;
            if (btn == null)
            {
                return;
            }
            this.frame_content.Source = new Uri(btn.Tag.ToString(),UriKind.Relative);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        #region 换肤
        private void men_ch_Click(object sender, RoutedEventArgs e)
        {
            ChangeLangue(true);
        }

        private void menu_us_Click(object sender, RoutedEventArgs e)
        {
            ChangeLangue(false);
        }

        private static void ChangeLangue(bool isChinese)
        {
            var znRes = Application.LoadComponent(new Uri("./Language/ZH_CN.xaml", UriKind.Relative)) as ResourceDictionary;
            var ENRes = Application.LoadComponent(new Uri("./Language/EN_US.xaml", UriKind.Relative)) as ResourceDictionary;
            if (isChinese)
            {
                Application.Current.Resources.MergedDictionaries.Remove(ENRes);
                Application.Current.Resources.MergedDictionaries.Add(znRes);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Remove(znRes);
                Application.Current.Resources.MergedDictionaries.Add(ENRes);
            }
        }
        #endregion
    }
}
