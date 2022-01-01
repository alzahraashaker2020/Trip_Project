using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Area
    {
        public Area()
        {
            AreaDiscount = new HashSet<AreaDiscount>();
            FavouriteArea = new HashSet<FavouriteArea>();
            RideDistinationAreaNavigation = new HashSet<Ride>();
            RideSourceAreaNavigation = new HashSet<Ride>();
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public virtual ICollection<AreaDiscount> AreaDiscount { get; set; }
        public virtual ICollection<FavouriteArea> FavouriteArea { get; set; }
        public virtual ICollection<Ride> RideDistinationAreaNavigation { get; set; }
        public virtual ICollection<Ride> RideSourceAreaNavigation { get; set; }
    }
}
