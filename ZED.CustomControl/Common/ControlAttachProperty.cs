using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ZED.CustomControl
{
    public static class ControlAttachProperty
    {
        #region border圆角
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ControlAttachProperty), new PropertyMetadata(null));
        #endregion

        #region 附加组件模板
        public static ControlTemplate GetAttachContent(DependencyObject obj)
        {
            return (ControlTemplate)obj.GetValue(AttachContentProperty);
        }

        public static void SetAttachContent(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(AttachContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for AttachContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachContentProperty =
            DependencyProperty.RegisterAttached("AttachContent", typeof(ControlTemplate), typeof(ControlAttachProperty), new PropertyMetadata(null));
        #endregion
    }
}
