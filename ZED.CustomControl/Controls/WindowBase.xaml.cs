using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ZED.CustomControl
{
    /// <summary>
    /// WindowBase.xaml 的交互逻辑
    /// </summary>
    public partial class WindowBase : Window
    {
        #region 依赖属性

        #region 标题栏图片和文字前景色
        public static readonly DependencyProperty CaptionForegroundProperty = DependencyProperty.Register(
    "CaptionForeground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(null));

        public Brush CaptionForeground
        {
            get { return (Brush)GetValue(CaptionForegroundProperty); }
            set { SetValue(CaptionForegroundProperty, value); }
        }
        #endregion

        #region 标题栏图标


        public string FIcon
        {
            get { return (string)GetValue(FIconProperty); }
            set { SetValue(FIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FIconProperty =
            DependencyProperty.Register("FIcon", typeof(string), typeof(WindowBase), new PropertyMetadata("\ue62e"));


        #endregion

        #region 标题栏图标大小


        public double FIconSize
        {
            get { return (double)GetValue(FIconSizeProperty); }
            set { SetValue(FIconSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FIconSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FIconSizeProperty =
            DependencyProperty.Register("FIconSize", typeof(double), typeof(WindowBase), new PropertyMetadata(30D));


        #endregion

        #region 标题栏标题大小


        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.Register("HeaderFontSize", typeof(double), typeof(WindowBase), new PropertyMetadata(15D));


        #endregion

        #region 标题栏背景


        public Brush CaptionBackground
        {
            get { return (Brush)GetValue(CaptionBackgroundProperty); }
            set { SetValue(CaptionBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaptionBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionBackgroundProperty =
            DependencyProperty.Register("CaptionBackground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(null));


        #endregion

        #region  标题栏高度
        public double CaptionHeight
        {
            get { return (double)GetValue(CaptionHeightProperty); }
            set { SetValue(CaptionHeightProperty, value); }
        }
        public static readonly DependencyProperty CaptionHeightProperty =
            DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowBase), new PropertyMetadata(15D));
        #endregion

        #region 最小化按钮是否显示
        public bool MinButtonVisibility
        {
            get { return (bool)GetValue(MinButtonVisibilityProperty); }
            set { SetValue(MinButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MinButtonVisibilityProperty =
            DependencyProperty.Register("MinButtonVisibility", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));
        #endregion

        #region 最大化按钮是否显示
        public bool MaxButtonVisibility
        {
            get { return (bool)GetValue(MaxButtonVisibilityProperty); }
            set { SetValue(MaxButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MaxButtonVisibilityProperty =
            DependencyProperty.Register("MaxButtonVisibility", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));
        #endregion


        #endregion

        #region 命令定义
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseWindowCommand { get; protected set; }
        /// <summary>
        /// 最小化命令
        /// </summary>
        public ICommand MinimizeWindowCommand { get; protected set; }
        /// <summary>
        /// 最大化命令
        /// </summary>
        public ICommand MaximizeWindowCommand { get; protected set; }
        #endregion


        public WindowBase()
        {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Height + 10;
            this.Style = this.FindResource("DefaultWindowBaseStyle") as Style;
            this.Title = "基本窗体";
            this.CloseWindowCommand = new RoutedUICommand();
            this.BindCommand(CloseWindowCommand, this.Close_Execute);
            this.MinimizeWindowCommand = new RoutedUICommand();
            this.BindCommand(MinimizeWindowCommand, this.Min_Execute);
            this.MaximizeWindowCommand = new RoutedUICommand();
            this.BindCommand(MaximizeWindowCommand, this.Max_Execute);
        }

        private void Max_Execute(object arg1, ExecutedRoutedEventArgs arg2)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            arg2.Handled = true;
        }

        private void Min_Execute(object arg1, ExecutedRoutedEventArgs arg2)
        {
            this.WindowState = WindowState.Minimized;
            arg2.Handled = true;
        }

        private void Close_Execute(object arg1, ExecutedRoutedEventArgs arg2)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
