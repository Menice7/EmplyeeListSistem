using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IscilerMaas.Models
{
    public class Odenis
    {
        public int IsciId { get; set; }
        public string IsciName { get; set; }
        public int IlId { get; set; }
        public int Il { get; set; }
        public List<Iller> Iller { get; set; }
        public List<Aylar> Aylar { get; set; }
        public int AyId { get; set; }
        public string Ay { get; set; }
        public int OdenisMiqdari { get; set; }
    }
}
