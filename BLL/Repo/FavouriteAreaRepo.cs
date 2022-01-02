using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
   public class FavouriteAreaRepo : Base_Repository<FavouriteArea>, IFavouriteArea
    {
        public FavouriteAreaRepo(TripContext dbContext) : base(dbContext)
        {

        }
    }
}
