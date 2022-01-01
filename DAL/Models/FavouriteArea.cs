using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class FavouriteArea
    {
        public int Id { get; set; }
        public int? AreaId { get; set; }
        public int? DriverId { get; set; }

        public virtual Area Area { get; set; }
        public virtual User Driver { get; set; }
    }
}
