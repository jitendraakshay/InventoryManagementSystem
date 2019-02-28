using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class UserMenu
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string ParentName { get; set; }
        public string MenuURI { get; set; }
        public int OrderBy { get; set; }
        public string Options { get; set; }
        public int ParentID { get; set; }
        public string IconClass { get; set; }
        //For Menu Role
        public string Access { get; set; }


       
    }

}