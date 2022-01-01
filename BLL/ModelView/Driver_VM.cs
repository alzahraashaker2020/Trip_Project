using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ModelView
{
   public class Driver_VM
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int sutatusSuspend { get; set; }
        public string nationalId { get; set; }
        public string licenceId { get; set; }
        public string password { get; set; }
        public int? roleId { get; set; }
    }
}
