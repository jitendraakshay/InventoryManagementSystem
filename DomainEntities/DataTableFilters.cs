using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DataTableFilters
    {
        private string _dir;
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string SearchKey { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection
        {
            get { return this._dir.ToUpper(); }
            set { this._dir = value; }
        }
    }
}
