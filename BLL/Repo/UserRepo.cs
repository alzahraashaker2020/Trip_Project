using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    class UserRepo : Base_Repo<User>, IUser
    {
        public UserRepo(TripContext dbContext) : base(dbContext)
        {
          
        }
        public List<int> GetAvalibleDriver(DateTime time)
        {
            
            return null;

        }
    }
}
