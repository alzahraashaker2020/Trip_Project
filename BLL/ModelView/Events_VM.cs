using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ModelView
{
   public class Events_VM
    {
        public string EventName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? EventDate { get; set; }
        public string ClientName { get; set; }
    }
}
