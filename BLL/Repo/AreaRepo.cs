using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    public class AreaRepo : Base_Repo<Area>, IArea
    {
        private readonly IRepoWrapper repo;
        private TripContext context;
        public AreaRepo(TripContext dbContext) : base(dbContext)
        {
            context = dbContext;

        }
    }
}
