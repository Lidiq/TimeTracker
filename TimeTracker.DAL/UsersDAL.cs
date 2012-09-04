using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker.DAL
{
    public class UsersDAL
    {
        public static void CreateUser(string Username, string FirstName, string LastName, string Position)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            aspnet_Users user = entity.aspnet_Users.First(el => el.UserName == Username);
            entity.AddToUsers(new Users()
            {
               Username = Username,
               UserID = user.UserId,
               FirstName = FirstName,
               LastName = LastName,
               Position = Position,
               IsActive = true
            });
            entity.SaveChanges();
        }

        public static Users GetUserByUsername(string username)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.Users.First(el => el.Username == username);
        }

        public static aspnet_Membership GetMembershipUserByUsername(string username)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Guid id = entity.Users.First(el => el.Username == username).UserID;
            return entity.aspnet_Membership.First(el => el.UserId == id);
        }

        public static List<aspnet_Users> GetAllMembershipUsers()
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.aspnet_Users.ToList();
        }


        /**
         * Get all active users from the database
         */ 
        public static List<Users> GetAllUsers()
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.Users.Where(el => el.IsActive).ToList();
        }


        /**
         * Get all the users from the database
         */
        public static List<Users> GetUsers()
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.Users.ToList();
        }


        /**
         * Get the number of logged hours for an user
         */ 
        public static int GetUserSpentTime(Guid userId)
        {
            //sum duration from workcards by userid
            TimeTrackerEntities entity = new TimeTrackerEntities();
            if (entity.WorkCards.Any(el => el.UserId == userId) == false)
            {
                return 0;
            }
            else
            {
                return entity.WorkCards.Where(el => el.UserId == userId).Sum(el => el.Duration);
            }
        }


        /**
         * Get the number of tasks assigned to a user
         */ 
        public static int GetUserNumberOfTasks(Guid userId)
        {
            //count userstasks by userid
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.UsersTasks.Count(el => el.UserId == userId);
        }


        /**
         * Delete user means change his status IsActive to false
         */
        public static void DeleteUser(string username)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Guid id = entity.aspnet_Users.First(el => el.UserName == username).UserId;
            Users user = entity.Users.First(el => el.UserID == id);
            user.IsActive = false;
            entity.SaveChanges();
            
        }


        /**
         * Edit user information, username cannot be edited
         */ 
        public static void EditUser(string username, string FirstName, string LastName, string email, string password)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Users user = entity.Users.First(el => el.Username == username);
            aspnet_Membership membershipUser = entity.aspnet_Membership.First(el => el.UserId == user.UserID);
            if (FirstName != "")
            {
                user.FirstName = FirstName;
            }
            if (LastName != "")
            {
                user.LastName = LastName;
            }
            if (email != "")
            {
                membershipUser.Email = email;
            }
            if (password != "")
            {
                membershipUser.Password = password;
            }
            entity.SaveChanges();
        }

       

        public static List<Users> GetUsersByTaskId(Guid taskId)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            List<UsersTasks> all = entity.UsersTasks.Select(el => el).Where(el => el.TaskId == taskId).ToList();

            List<Guid> userIds = all.Select(el => el.UserId).ToList();
            return entity.Users.Select(el => el).Where(id => userIds.Contains(id.UserID) && id.IsActive).ToList(); 
        }


        public static int GetUserSpentTimeOnTask(Guid userId, Guid taskId)
        {
            //sum duration from workcards where taskid and userid == 
            TimeTrackerEntities entity = new TimeTrackerEntities();
            if (entity.WorkCards.Any(el => el.TaskId == taskId && el.UserId == userId) == false)
            {
                return 0;
            }
            else
            {
                return entity.WorkCards.Where(el => el.TaskId == taskId && el.UserId == userId).Sum(el => el.Duration);
            }
        }
    }
}
