using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTracker.DAL;

namespace TimeTracker.Models
{
    public class UserViewModel
    {
        public Guid userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string username { get; set; }
        public bool isActive { get; set; }
        public string position { get; set; }


        public UserViewModel(Users user)
        {
            this.userId = user.UserID;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.username = user.Username;
            this.position = user.Position;
            this.isActive = user.IsActive;
        }

    }
}