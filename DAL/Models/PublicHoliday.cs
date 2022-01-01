using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class PublicHoliday
    {
        public int Id { get; set; }
        public DateTime? HolidayDate { get; set; }
        public string Note { get; set; }
    }
}
