﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class ReturnType
    {
        public bool Result { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string ProfileImage { get; set; }
        public bool AllowLogin { get; set; }
        public string RoleID { get; set; }
        public bool isSuperUser { get; set; }
        public string ImageType { get; set; }
    }
}
