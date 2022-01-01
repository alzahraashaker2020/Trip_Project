using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class User
    {
        public User()
        {
            FavouriteArea = new HashSet<FavouriteArea>();
            Notification = new HashSet<Notification>();
            Offer = new HashSet<Offer>();
            RateClient = new HashSet<Rate>();
            RateDriver = new HashSet<Rate>();
            RideClient = new HashSet<Ride>();
            RideDriver = new HashSet<Ride>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int SutatusSuspend { get; set; }
        public string NationalId { get; set; }
        public string LicenceId { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public int? RoleId { get; set; }

        public virtual Roles Role { get; set; }
        public virtual ICollection<FavouriteArea> FavouriteArea { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Offer> Offer { get; set; }
        public virtual ICollection<Rate> RateClient { get; set; }
        public virtual ICollection<Rate> RateDriver { get; set; }
        public virtual ICollection<Ride> RideClient { get; set; }
        public virtual ICollection<Ride> RideDriver { get; set; }
    }
}
