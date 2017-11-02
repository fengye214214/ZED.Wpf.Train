using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZED.Train.Entiry
{
    public class Channel
    {
        public string Channelname { get; set; }

        public string Channenum { get; set; }

        public override string ToString()
        {
            return string.Format("[通道号:{0} 通道名:{1}]", Channenum, Channelname);
        }
    }
}
