using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class AreaDiscount
    {
        public int Id { get; set; }
        public double? Value { get; set; }
        public int? AreaId { get; set; }
        public string Note { get; set; }

        public virtual Area Area { get; set; }
    }
}
