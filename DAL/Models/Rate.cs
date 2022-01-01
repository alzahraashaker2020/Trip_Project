using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Rate
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Review { get; set; }
        public int? ClientId { get; set; }
        public int? DriverId { get; set; }

        public virtual User Client { get; set; }
        public virtual User Driver { get; set; }
    }
}
