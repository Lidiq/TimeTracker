using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker.DAL
{
    public class TasksDAL
    {
        public static void CreateTask(string TaskName, string Description, DateTime start, DateTime end, int estimation, string status, string[] userIds)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Guid taskId = Guid.NewGuid();
            entity.AddToTasks(new Tasks()
            {
                TaskId = taskId,
                TaskName = TaskName,
                Description = Description,
                StartTime = start,
                EndTime = end,
                Estimation = estimation,
                Status = status,
                IsActive = true
            });
            entity.SaveChanges();


            foreach (var user in userIds)
            {
                entity.AddToUsersTasks(new UsersTasks()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(user),
                    TaskId = taskId
                });
                entity.SaveChanges();
            }
        }


        public static Tasks GetTaskById(Guid id)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.Tasks.First(el => el.TaskId == id);
        }

        public static List<Tasks> GetAllTasks()
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.Tasks.Where(el => el.IsActive).ToList();
        }

        /**
         * Get all the tasks, active and inactive
         */ 
        public static List<Tasks> GetTasks()
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            return entity.Tasks.ToList();
        }

        /**
         * Delete task means change its status IsActive to false
         */
        public static void DeleteTask(Guid id)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Tasks task = entity.Tasks.First(el => el.TaskId == id);
            task.IsActive = false;
            entity.SaveChanges();
        }


        /**
         * Edit task information
         */
        public static void EditTask(Guid id, string TaskName, string Description, DateTime start, DateTime end, int estimation, string status)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Tasks task = entity.Tasks.First(el => el.TaskId == id);
            task.TaskName = TaskName;
            task.Description = Description;
            task.StartTime = start;
            task.EndTime = end;
            task.Estimation = estimation;
            task.Status = status;
            entity.SaveChanges();
        }


        /**
         * Get tasks by given username
         */ 
        public static List<Tasks> GetTasksByUser(string username)
        {
            TimeTrackerEntities entity = new TimeTrackerEntities();
            Guid userId = entity.Users.First(el => el.Username == username).UserID;

            List<UsersTasks> tasks = entity.UsersTasks.Select(el => el).Where(item => item.UserId == userId).ToList();
            List<Guid> taskIds = tasks.Select(el => el.TaskId).ToList();
            return entity.Tasks.Select(el => el).Where(id => taskIds.Contains(id.TaskId) && id.IsActive).ToList(); 
        }
    }
}
