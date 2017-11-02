using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZED.Train.Entiry
{
    public class DeviceNode
    {
        public DeviceNode()
        {
            Channels = new List<Channel>();
        }

        public string AlarmIp { get; set; }

        public string AlarmPort { get; set; }

        public List<Channel> Channels { get; set; }

        public string ChannelsName { get; set; }

        public string Deviceaccount { get; set; }

        public string Deviceid { get; set; }

        public string Devicename { get; set; }

        public string Deviceuse { get; set; }

        public string DeviceuseName { get; set; }

        public string ParentNodeId { get; set; }

        public string Phonenum { get; set; }

        public string PpvsIp { get; set; }

        public string PpvsPort { get; set; }

        public string RsmIp { get; set; }

        public string RsmPort { get; set; }

        public string Serverroute { get; set; }

        public string ServerrouteName { get; set; }

        public string Streamtype { get; set; }

        public string StreamTypeName { get; set; }

        public string Streamusertype { get; set; }

        public string Transmitmode { get; set; }

        public string TransmitmodeName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Deviceid, Deviceaccount, Devicename);
        }

    }
}
