using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Units
    {
        public int? UnitID { get; set; }
        public string UnitName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string EntryBy { get; set; }
        public string EntryDate { get; set; }
    }
}
