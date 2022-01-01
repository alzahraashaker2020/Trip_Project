using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    class NotificationRepo : Base_Repo<Notification>, INotification
    {
        public NotificationRepo(TripContext dbContext) : base(dbContext)
        {

        }
    }
}
