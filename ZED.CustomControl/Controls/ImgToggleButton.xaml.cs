using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZED.CustomControl
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class ImgToggleButton : ToggleButton
    {

        #region 依赖属性定义

        #region 按钮图片文字

        /// <summary>
        /// 按钮图片文字
        /// </summary>
        public string FIcon
        {
            get { return (string)GetValue(FIconProperty); }
            set { SetValue(FIconProperty, value); }
        }
        public static readonly DependencyProperty FIconProperty =
            DependencyProperty.Register("FIcon", typeof(string), typeof(ImgToggleButton), new PropertyMetadata("\ue604"));
        #endregion

        #region 按钮图片边距

        /// <summary>
        /// 按钮图片边距
        /// </summary>
        public Thickness FIconMargin
        {
            get { return (Thickness)GetValue(FIconMarginProperty); }
            set { SetValue(FIconMarginProperty, value); }
        }
        public static readonly DependencyProperty FIconMarginProperty =
            DependencyProperty.Register("FIconMargin", typeof(Thickness), typeof(ImgToggleButton), new PropertyMetadata(new Thickness(5, 0, 2, 0)));
        #endregion

        #region 按钮图片大小

        /// <summary>
        /// 按钮图片大小
        /// </summary>
        public int FIconSize
        {
            get { return (int)GetValue(FIconSizeProperty); }
            set { SetValue(FIconSizeProperty, value); }
        }
        public static readonly DependencyProperty FIconSizeProperty =
            DependencyProperty.Register("FIconSize", typeof(int), typeof(ImgToggleButton), new PropertyMetadata(20));
        #endregion

        #region 按钮悬浮背景色
        /// <summary>
        /// 按钮悬浮背景色
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(ImgToggleButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 121, 214))));
        #endregion

        #region 按钮悬浮图标颜色
        /// <summary>
        ///  按钮悬浮图标颜色
        /// </summary>
        public Brush MouseOverFIconForeground
        {
            get { return (Brush)GetValue(MouseOverFIconForegroundProperty); }
            set { SetValue(MouseOverFIconForegroundProperty, value); }
        }
        public static readonly DependencyProperty MouseOverFIconForegroundProperty =
            DependencyProperty.Register("MouseOverFIconForeground", typeof(Brush), typeof(ImgToggleButton), new PropertyMetadata(Brushes.White));
        #endregion

        #region 按钮圆角
        /// <summary>
        /// 按钮圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ImgToggleButton), new PropertyMetadata(new CornerRadius(2)));
        /// <summary>
        /// 按钮圆角大小,左上，右上，右下，左下
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region 

        #endregion 图片是否已动画显示
        /// <summary>
        /// 图片是否已动画显示
        /// </summary>
        public bool IsAnimation
        {
            get { return (bool)GetValue(IsAnimationProperty); }
            set { SetValue(IsAnimationProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationProperty =
            DependencyProperty.Register("IsAnimation", typeof(bool), typeof(ImgToggleButton), new PropertyMetadata(false));
        #endregion


        #region 构造方法
        static ImgToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImgToggleButton), new FrameworkPropertyMetadata(typeof(ImgToggleButton)));
        }

        public ImgToggleButton()
        {
        }
        #endregion
    }
}
