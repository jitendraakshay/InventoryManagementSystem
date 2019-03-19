using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Products
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public bool IsProductActive { get; set; }
        public string EntryBy { get; set; }
        public string EntryDate { get; set; }
    }
    public class Attributes
    {
        public int? OptionID { get; set; }
        public int? ProductID { get; set; }
        public string OptionName { get; set; }
        public string OptionDescription { get; set; }
        public bool IsOptionActive { get; set; }
        public string EntryBy { get; set; }
        public string EntryDate { get; set; }

    }

    public class AttributeValue
    {
        public int? OptionID { get; set; }
        public string OptionName { get; set; }
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string ValueName { get; set; }
        public int? ValueID { get; set; }
        public bool IsValueActive { get; set; }
    }
    public class SKUs
    {
        public int? OptionID { get; set; }
        public string OptionName { get; set; }
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string ValueName { get; set; }
        public int? ValueID { get; set; }
        public int? SKUID { get; set; }
        public string SKU { get; set; }
    }
}
