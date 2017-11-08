using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZED.CustomControl
{
    /// <summary>
    /// MessageBoxEx.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxEx : WindowBase
    {
        public bool DialogResultEx { get; private set; }

        public MessageBoxEx()
        {
            InitializeComponent();
        }

        public MessageBoxEx(NotifyTypeEnum type, string msg) : this()
        {
            switch (type)
            {
                case NotifyTypeEnum.Tooltip:
                    this.FIcon = "\ue659";
                    break;
                case NotifyTypeEnum.Error:
                    this.BorderBrush = Brushes.Red;
                    this.CaptionForeground = Brushes.Red;
                    this.FIcon = "\ue644";
                    break;
                case NotifyTypeEnum.Warning:
                    this.BorderBrush = new SolidColorBrush(Color.FromRgb(202, 81, 0));
                    this.CaptionForeground = new SolidColorBrush(Color.FromRgb(202, 81, 0));
                    this.FIcon = "\ue60b";
                    break;
                case NotifyTypeEnum.Question:
                    this.FIcon = "\ue60e";
                    this.btn_cancel.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            txt_msg.Text = msg;
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResultEx = false;
            this.Close();
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResultEx = true;
            this.Close();
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="owner"></param>
        public static void ShowInfo(string msg, Window owner = null)
        {
            Show(NotifyTypeEnum.Tooltip, msg, owner);
        }

        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="owner"></param>
        public static void ShowWarn(string msg, Window owner = null)
        {
            Show(NotifyTypeEnum.Warning, msg, owner);
        }

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="owner"></param>
        public static void ShowError(string msg, Window owner = null)
        {
            Show(NotifyTypeEnum.Error, msg, owner);
        }

        /// <summary>
        /// 显示询问
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="owner"></param>
        public static bool ShowQuestion(string msg, Window owner = null)
        {
            return Show(NotifyTypeEnum.Question, msg, owner);
        }

        private static bool Show(NotifyTypeEnum type, string msg, Window owner = null)
        {
            var result = true;
            //此处不能用BeginInvoke异步执行
            Application.Current.Dispatcher.Invoke(new Action(() => 
            {
                var winMsg = new MessageBoxEx(type, msg);
                winMsg.Title = type.GetDescription();
                winMsg.Owner = owner ?? ComControlHelper.GetTopWindow();
                winMsg.ShowDialog();
                result = winMsg.DialogResultEx;
            }));
            return result;
        }
    }

    public enum NotifyTypeEnum
    {
        /// <summary>
        /// 提示
        /// </summary>
        [Description("提示")]
        Tooltip,

        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,

        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning,

        /// <summary>
        /// 询问
        /// </summary>
        [Description("询问")]
        Question
    }
}
