using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    class RolesRepo : Base_Repo<Roles>, IRoles
    {
        public RolesRepo(TripContext dbContext) : base(dbContext)
        {

        }
    }
}
