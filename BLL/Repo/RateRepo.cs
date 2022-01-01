using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
   public class RateRepo : Base_Repo<Rate>, IRate
    {
        public RateRepo(TripContext dbContext) : base(dbContext)
        {

        }

    }
}
