﻿using BLL.IRepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repo
{
    public class EventRepo : Base_Repo<Event>, IEvent
    {
        public EventRepo(TripContext dbContext) : base(dbContext)
        {

        }

    }
}
