using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Offer
    {
        public int Id { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }
        public int? RideId { get; set; }
        public int? DriverId { get; set; }

        public virtual User Driver { get; set; }
        public virtual Ride Ride { get; set; }
    }
}
