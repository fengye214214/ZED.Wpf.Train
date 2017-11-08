using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ZED.CustomControl
{   
    /// <summary>
    /// 系统转换
    /// </summary>
    public sealed class SystemConverter
    {   
        public static BooleanToVisibilityConverter BooleanToVisibilityConverter
        {
            get
            {
                return Singleton<BooleanToVisibilityConverter>.GetInstance();
            }
        }
    }
}
