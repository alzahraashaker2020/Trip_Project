using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Roles
    {
        public Roles()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
