using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string NotificationName { get; set; }
        public bool? SeenStatus { get; set; }
        public DateTime? NotifDate { get; set; }
        public string Note { get; set; }
        public int? OperationId { get; set; }

        public virtual User User { get; set; }
    }
}
