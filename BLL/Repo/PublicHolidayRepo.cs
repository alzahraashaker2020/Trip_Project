using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    class PublicHolidayRepo : Base_Repo<PublicHoliday>, IPublicHoliday
    {
        public PublicHolidayRepo(TripContext dbContext) : base(dbContext)
        {

        }
    }
}
