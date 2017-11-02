using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZED.Train.Entiry
{
    public class GpsModel
    {
        public string DeviceAccount { get; set; }

        public string DeviceStatus  { get; set; }

        public string DeviceStatusName { get; set; }

        public string Direction  { get; set; }

        public string DivisionEW  { get; set; }

        public string DivisionNS { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string ReceiveTime { get; set; }

        public string Speed { get; set; }
    }
}
