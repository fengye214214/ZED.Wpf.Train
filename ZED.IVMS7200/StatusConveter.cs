
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZED.Train.Entiry;

namespace ZED.IVMS7200
{
    public class StatusConveter
    {
        public static string DeviceStatusName(string str)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(str.Trim()))
                return result;

            if ("0".Equals(str))
                result = string.Format("离线 {0}", str);
            else if ("1".Equals(str))
                result = string.Format("在线 {0}", str);
            return result;
        }

        public static string ChannelsName(List<Channel> channelList)
        {
            string result = string.Empty;

            if (channelList == null || channelList.Count <= 0)
                return result;

            var resultList = new List<string>();
            foreach (var item in channelList)
            {   
                string str = string.Format("[通道号:{0} 通道名:{1}]", item.Channenum, item.Channelname);
                resultList.Add(str);
            }

            return string.Join(" ", resultList);
        }

        public static string TransmitmodeName(string code)
        {
            string result = string.Empty;

            if(string.IsNullOrEmpty(code.Trim()))
                return result;

            if ("0".Equals(code))
                result = string.Format("UDP传输 {0}", code);
            else if ("1".Equals(code))
                result = string.Format("TCP传输 {0}", code);
            return result;
        }

        public static string StreamTypeName(string type)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(type.Trim()))
                return result;

            if ("0".Equals(type))
                result = string.Format("主码流 {0}", type);
            else if ("1".Equals(type))
                result = string.Format("子码流 {0}", type);
            return result;
        }

        public static string DeviceuseName(string type)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(type.Trim()))
                return result;

            if ("0".Equals(type))
                result = string.Format("普通设备 {0}", type);
            else if ("1".Equals(type))
                result = string.Format("单兵设备 {0}", type);
            else if ("2".Equals(type))
                result = string.Format("车载设备 {0}", type);
            return result;
        }
    }
}
