using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public int? RideId { get; set; }
        public DateTime? EventDate { get; set; }
        public string Note { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
