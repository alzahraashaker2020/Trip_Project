using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ModelView
{
   public class Users_VM
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int? roleId { get; set; }
    }
}
