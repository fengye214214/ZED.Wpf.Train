using Microsoft.Win32;
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
    /// ExComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class ExComboBox : ComboBox
    {
        #region 构造方法
        static ExComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExComboBox), new FrameworkPropertyMetadata(typeof(ExComboBox)));
        }

        public ExComboBox()
        {
            this.IsEditable = true;
        }
        #endregion
    }
}
