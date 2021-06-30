using OrganizerAPI.Models.Common;
using System;

namespace OrganizerAPI.Models.Models
{
    public class UserTask : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public int UserId { get; set; }
    }
}
