using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    public class AreaDiscountRepo: Base_Repository<AreaDiscount>, IAreaDiscount
    {
        public AreaDiscountRepo(TripContext dbContext) : base(dbContext)
        {

        }
    }
}
