using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ZED.Train.Common;
using ZED.Train.Entiry;

namespace ZED.IVMS7200
{   
    /// <summary>
    /// 
    /// </summary>
    public class DeviceService : IDevice
    {
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="line">线路号</param>
        public DevTreeInfo GetDevTreeInfo(string userName, string pwd, uint line)
        {   
            var devTree = new IVMS7200.GetDevTreeServiceClient();
            var IdevTree = (ICommunicationObject)devTree;
            using (IdevTree.AsDisposable())
            {
                var strPwd = MD5Encrypt.GetDigest(pwd);
                var result = devTree.getDevTreeInfo(userName, strPwd, Convert.ToInt16(line));
                return GetData(result);
            }
        }

        #region 内部方法
        private static DevTreeInfo GetData(IVMS7200.DevTreeInfoResult result)
        {
            DevTreeInfo devList = new DevTreeInfo();
            devList.Message = result.message;
            devList.MessageCode = result.messageCode.ToString();

            foreach (var item in result.areaList)
            {
                var areaModel = new Area()
                {
                    AreaId = item.areaId.ToString(),
                    AreaName = item.areaName,
                    ParentAreaId = item.parentAreaId.ToString(),
                    Type = item.type.ToString()
                };
                devList.AreaList.Add(areaModel);
            }

            foreach (var item in result.deviceNodeList)
            {
                var deviceModel = new DeviceNode()
                {
                    AlarmIp = item.alarmIp,
                    AlarmPort = item.alarmPort.ToString(),
                    Deviceaccount = item.deviceaccount,
                    Deviceid = item.deviceid.ToString(),
                    Devicename = item.devicename,
                    Deviceuse = item.deviceuse.ToString(),
                    ParentNodeId = item.parentNodeId.ToString(),
                    Phonenum = item.phonenum,
                    PpvsIp = item.ppvsIp,
                    PpvsPort = item.ppvsPort.ToString(),
                    RsmIp = item.rsmIp,
                    RsmPort = item.rsmPort.ToString(),
                    Serverroute = item.serverroute.ToString(),
                    Streamtype = item.streamtype.ToString(),
                    Streamusertype = item.streamusertype.ToString(),
                    Transmitmode = item.transmitmode.ToString()
                };

                foreach (var item1 in item.channels)
                {
                    var channelModel = new Channel()
                    {
                        Channelname = item1.channelname,
                        Channenum = item1.channenum.ToString()
                    };
                    deviceModel.Channels.Add(channelModel);
                }

                deviceModel.ChannelsName = StatusConveter.ChannelsName(deviceModel.Channels);
                deviceModel.TransmitmodeName = StatusConveter.TransmitmodeName(deviceModel.Transmitmode);
                deviceModel.StreamTypeName = StatusConveter.StreamTypeName(deviceModel.Streamtype);
                deviceModel.DeviceuseName = StatusConveter.DeviceuseName(deviceModel.Deviceuse);

                devList.DeviceNodeList.Add(deviceModel);
            }

            return devList;
        }
        #endregion
    }
}
