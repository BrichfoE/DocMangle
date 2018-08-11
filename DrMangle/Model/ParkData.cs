using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle.Model
{
    public class ParkData
    {
        public string ParkName { get; set; }
        public int ParkPart { get; set; }
        public LinkedList<PartData> PartsList { get; set; }
    
        public ParkData(string name, int part)
        {
            ParkName = name;
            ParkPart = part;
            PartsList = new LinkedList<PartData>();
        }
    }
}
