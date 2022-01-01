using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Repo
{
   public class RideRepo : Base_Repo<Ride>, IRide
    {
        public RideRepo(TripContext dbContext) : base(dbContext)
        {

        }
       
    }
}
