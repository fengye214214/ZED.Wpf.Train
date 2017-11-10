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

namespace ZED.CustomControl
{
    /// <summary>
    /// TextBoxWatermark.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxWatermark : TextBox
    {
        #region 依赖属性

        #region 水印
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(TextBoxWatermark), new PropertyMetadata(""));
        #endregion

        public AttratchButton AttButtonType
        {
            get { return (AttratchButton)GetValue(AttButtonTyleProperty); }
            set { SetValue(AttButtonTyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AttButtonType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttButtonTyleProperty =
            DependencyProperty.Register("AttButtonType", typeof(AttratchButton), typeof(TextBoxWatermark), new PropertyMetadata(AttratchButton.Clear, att_changed));

        private static void att_changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBoxWatermark;
            if (e.NewValue != null)
            {
                AttratchButton ab = (AttratchButton)(Convert.ToInt32(e.NewValue));
                switch (ab)
                {
                    case AttratchButton.Clear:
                        control.Style = control.FindResource("ClearButtonTextBox") as Style;
                        break;
                    case AttratchButton.OpenFilePathDialog:
                        control.Style = control.FindResource("OpenFileTextBox") as Style;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Commond命令
        /// <summary>
        /// 清除输入框Text事件命令
        /// </summary>
        public ICommand ClearTextCommand { get; protected set; }
        #endregion

        #region 构造函数
        static TextBoxWatermark()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxWatermark), new FrameworkPropertyMetadata(typeof(TextBoxWatermark)));
        }
        public TextBoxWatermark()
        {
            this.ClearTextCommand = new RoutedUICommand();
            this.BindCommand(ClearTextCommand, this.Clear_Execute);
        }
        #endregion

        #region 清除文本框内容
        private void Clear_Execute(object arg1, ExecutedRoutedEventArgs arg2)
        {
            var element = arg2.Source as FrameworkElement;
            if (element == null)
            {
                return;
            }
            if ((element as TextBox) != null)
            {
                (element as TextBox).Clear();
            }

            
        }
        #endregion
    }

    public enum AttratchButton
    {
        Clear = 0,

        OpenFilePathDialog = 1
    }

}
