using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repo
{
    public class RepoWrapper : IRepoWrapper
    {
        private TripContext _contex;
        private IArea area;
        private IAreaDiscount areaDiscount;
        private IEvent Event;
        private IFavouriteArea favouriteArea;
        private INotification notification;
        private IOffer offer;
        private IPublicHoliday publicHoliday;
        private IRate rate;
        private IRide ride;
        private IRoles roles;
        private IUser user;

        public RepoWrapper(TripContext context)
        {
            _contex = context;
        }



        public IArea _Area
        {
            get
            {
                if (area == null) area = new AreaRepo(_contex); return area;
            }
        }
        public IAreaDiscount _AreaDiscount { get { if (areaDiscount == null) areaDiscount = new AreaDiscountRepo(_contex); return areaDiscount; } }

        public IEvent _Event { get { if (Event == null) Event = new EventRepo(_contex); return Event; } }
        public IFavouriteArea _FavouriteArea { get { if (favouriteArea == null) favouriteArea = new FavouriteAreaRepo(_contex); return favouriteArea; } }
        public INotification _Notification { get { if (notification == null) notification = new NotificationRepo(_contex); return notification; } }
        public IOffer _Offer { get { if (offer == null) offer = new OfferRepo(_contex); return offer; } }
        public IPublicHoliday _PublicHoliday { get { if (publicHoliday == null) publicHoliday = new PublicHolidayRepo(_contex); return publicHoliday; } }
        public IRate _Rate { get { if (rate == null) rate = new RateRepo(_contex); return rate; } }
        public IRide _Ride { get { if (ride == null) ride = new RideRepo(_contex); return ride; } }
        public IRoles _Roles { get { if (roles == null) roles = new RolesRepo(_contex); return roles; } }
        public IUser _User { get { if (user == null) user = new UserRepo(_contex); return user; } }

      

        public  void Save()
        {
            _contex.SaveChanges();
        }
    }
}
