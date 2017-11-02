using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZED.Train.Entiry
{
    public class Area
    {
        public string AreaId { get; set; }

        public string AreaName { get; set; }

        public string ParentAreaId { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", AreaId, AreaName);
        }
    }
}
