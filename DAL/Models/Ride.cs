using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Ride
    {
        public Ride()
        {
            Event = new HashSet<Event>();
            Offer = new HashSet<Offer>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public double? Price { get; set; }
        public int? PassengerNo { get; set; }
        public double? DiscountVal { get; set; }
        public int? ClientId { get; set; }
        public int? SourceArea { get; set; }
        public int? DistinationArea { get; set; }
        public int? DriverId { get; set; }
        public int? RideState { get; set; }

        public virtual User Client { get; set; }
        public virtual Area DistinationAreaNavigation { get; set; }
        public virtual User Driver { get; set; }
        public virtual Area SourceAreaNavigation { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<Offer> Offer { get; set; }
    }
}
