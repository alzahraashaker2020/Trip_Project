using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    class OfferRepo : Base_Repository<Offer>, IOffer
    {
        public OfferRepo(TripContext dbContext) : base(dbContext)
        {

        }
    }
}
