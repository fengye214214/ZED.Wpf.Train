using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZED.Train.Common
{
    public class ResponseCmd<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public T data { get; set; }

        public static ResponseCmd<T> CreateResponseCmd(string data)
        {
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            //日期类型默认格式化处理
            setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            //setting.DateFormatString = "yyyy-MM-dd";
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //空值处理
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<ResponseCmd<T>>(data, setting);
        }

    }
}
