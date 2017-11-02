using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZED.Train.Entiry
{
    public class DevTreeInfo
    {
        public DevTreeInfo()
        {
            DeviceNodeList = new List<DeviceNode>();
            AreaList = new List<Area>();
        }

        public string UserName { get; set; }

        public string Line { get; set; }

        public List<Area> AreaList  { get; set; }

        public List<DeviceNode> DeviceNodeList  { get; set; }

        public string Message  { get; set; }

        public string MessageCode  { get; set; }
    }
}
