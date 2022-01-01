using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepo
{
   public interface IRepoWrapper
    {
        IArea _Area { get; }
        IAreaDiscount _AreaDiscount { get; }
        IEvent _Event { get; }
        IFavouriteArea _FavouriteArea { get; }
        INotification _Notification { get; }
        IOffer _Offer { get; }
        IPublicHoliday _PublicHoliday { get; }
        IRate _Rate { get; }
        IRide _Ride { get; }
        IRoles _Roles { get; }
        IUser _User { get; }

        void Save();

    }
}
