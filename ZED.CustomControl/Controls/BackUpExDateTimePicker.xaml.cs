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
using Xceed.Wpf.Toolkit;

namespace com.zed3.NewSH
{
    /// <summary>
    /// BackUpExDateTimePicker.xaml 的交互逻辑
    /// 2017-9-14 add by fy
    /// </summary>
    [TemplatePart(Name = "PART_Calendar", Type = typeof(Calendar))]
    public partial class BackUpExDateTimePicker : DateTimePicker
    {
        #region 依赖属性

        #region 是否作为开始日期控件
        /// <summary>
        /// 是否作为开始日期控件
        /// </summary>
        public bool IsStartTime
        {
            get { return (bool)GetValue(IsStartTimeProperty); }
            set { SetValue(IsStartTimeProperty, value); }
        }

        public static readonly DependencyProperty IsStartTimeProperty =
            DependencyProperty.Register("IsStartTime", typeof(bool), typeof(BackUpExDateTimePicker), new PropertyMetadata(false));
        #endregion

        #region 是否作为结束日期控件
        /// <summary>
        /// 是否作为结束日期控件
        /// </summary>
        public bool IsEndTime
        {
            get { return (bool)GetValue(IsEndTimeProperty); }
            set { SetValue(IsEndTimeProperty, value); }
        }

        public static readonly DependencyProperty IsEndTimeProperty =
            DependencyProperty.Register("IsEndTime", typeof(bool), typeof(BackUpExDateTimePicker), new PropertyMetadata(false));


        /// <summary>
        /// 选择的日期
        /// </summary>
        private string formatText = String.Empty;

        public string FormatText
        {
            get
            {
                var result = string.Empty;
                if (this.Value != null && this.Value.HasValue)
                {
                    result = this.Value.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return result;
            }
        }

        #endregion

        #region 最小日期
        public DateTime? MinDateTime
        {
            get { return (DateTime?)GetValue(MinDateTimeProperty); }
            set { SetValue(MinDateTimeProperty, value); }
        }

        public static readonly DependencyProperty MinDateTimeProperty =
            DependencyProperty.Register("MinDateTime", typeof(DateTime?), typeof(BackUpExDateTimePicker), new PropertyMetadata(null, MinDateTimeChanged));

        private static void MinDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BackUpExDateTimePicker;
            try
            {
                if (e.NewValue != null)
                {

                    DateTime? date = Convert.ToDateTime(e.NewValue);
                    if (date != null && control.Minimum.HasValue && date.HasValue)
                    {
                        var date1 = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
                        var date2 = new DateTime(control.Minimum.Value.Year, control.Minimum.Value.Month, control.Minimum.Value.Day);

                        if (date2.Year == 1 && date2.Month == 1 && date2.Day == 1)
                        {
                            control.Minimum = date.Value;
                        }
                        else
                        {
                            //新的日期要小于当前控件的最小值
                            //if (DateTime.Compare(date1, date2) <= 0)
                            //{
                            //    control.Minimum = date.Value;
                            //}
                            control.Minimum = date.Value;
                        }
                    }
                }
                else
                {
                    control.Minimum = DateTime.MinValue;
                }
            }
            catch { }
        }
        #endregion

        #region 最大日期
        public DateTime? MaxDateTime
        {
            get { return (DateTime?)GetValue(MaxDateTimeProperty); }
            set { SetValue(MaxDateTimeProperty, value); }
        }

        public static readonly DependencyProperty MaxDateTimeProperty =
            DependencyProperty.Register("MaxDateTime", typeof(DateTime?), typeof(BackUpExDateTimePicker), new PropertyMetadata(null, MaxDateChanged));

        private static void MaxDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BackUpExDateTimePicker;
            try
            {
                if (e.NewValue != null)
                {
                    DateTime? date = Convert.ToDateTime(e.NewValue);
                    if (date != null && control.Maximum.HasValue && date.HasValue)
                    {
                        var date1 = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
                        var date2 = new DateTime(control.Maximum.Value.Year, control.Maximum.Value.Month, control.Maximum.Value.Day);

                        if (date2.Year == 9999 && date2.Month == 12 && date2.Day == 31)
                        {
                            control.Maximum = date.Value;
                        }
                        else
                        {
                            //新的日期要大于当前控件的最大值
                            //if (DateTime.Compare(date1, date2) >= 0)
                            //{
                            //    control.Maximum = date.Value;
                            //}
                            control.Maximum = date.Value;
                        }
                    }
                }
                else
                {
                    control.Maximum = DateTime.MaxValue;
                }
            }
            catch { }
        }
        #endregion

        public DateTime? NewValue
        {
            get { return (DateTime?)GetValue(NewValueProperty); }
            set { SetValue(NewValueProperty, value); }
        }

        public static readonly DependencyProperty NewValueProperty =
            DependencyProperty.Register("NewValue", typeof(DateTime?), typeof(BackUpExDateTimePicker), new PropertyMetadata(null));

        /// <summary>
        /// 文本是否为空
        /// </summary>
        public bool IsTextEmpty
        {
            get { return (bool)GetValue(IsTextEmptyProperty); }
            set { SetValue(IsTextEmptyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTextEmpty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTextEmptyProperty =
            DependencyProperty.Register("IsTextEmpty", typeof(bool), typeof(BackUpExDateTimePicker), new PropertyMetadata(true));


        #endregion

        #region 构造方法
        static BackUpExDateTimePicker()
        {

        }

        public BackUpExDateTimePicker()
        {
            this.Style = this.FindResource("DateTimePickerStyle1") as Style;

            this.Loaded += ExDateTimePicker_Loaded;
            this.ValueChanged += ExDateTimePicker_ValueChanged;

            this.UpdateValueOnEnterKey = false;
            this.TimeFormat = DateTimeFormat.LongTime;


            this.ClipValueToMinMax = true;
            //this.AllowTextInput = false;
            this.ShowButtonSpinner = false;
            this.Format = DateTimeFormat.Custom;
            this.AllowSpin = false;
            this.TimePickerAllowSpin = false;
            this.TimePickerShowButtonSpinner = false;
            this.FormatString = "yyyy-MM-dd HH:mm:ss";

        }
        #endregion

        #region 方法
        void ExDateTimePicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.Value == null && !IsTextEmpty)
            {
                this.Value = DateTime.Now;
            }
            NewValue = this.Value;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var cal = Template.FindName("PART_Calendar", this) as Calendar;
            cal.SelectedDatesChanged += cal_SelectedDatesChanged;

        }

        void ExDateTimePicker_Loaded(object sender, RoutedEventArgs e)
        {
            InitStartAndEndTime();
        }

        private void InitStartAndEndTime()
        {
            try
            {
                var dateTimeNow = DateTime.Now;

                if (IsTextEmpty)
                {
                    this.Value = null;
                }
                if (IsEndTime && !IsTextEmpty)
                {
                    this.Value = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 23, 59, 59);
                }

                if (IsStartTime && !IsTextEmpty)
                {
                    if (DateTime.Compare(DateTime.Now, this.Maximum.Value) > 0)
                    {
                        this.Maximum = DateTime.Now;
                    }
                    this.Value = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 0, 0, 0);
                }
            }
            catch { }
        }

        void cal_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var cal = sender as Calendar;
            var dateTime = cal.SelectedDate;
            if (dateTime != null && dateTime.HasValue)
            {
                if (IsEndTime)
                {
                    this.Value = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, 23, 59, 59);
                }

                if (IsStartTime)
                {
                    this.Value = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, 0, 0, 0);
                }
            }
        }

        public void ResertDate()
        {
            InitStartAndEndTime();
        }

        #endregion
    }
}
