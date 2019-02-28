using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SendMail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SMTPHost { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }

    }
}
