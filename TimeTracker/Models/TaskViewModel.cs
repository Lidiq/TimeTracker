using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTracker.DAL;

namespace TimeTracker.Models
{
    public class TaskViewModel
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public string Status { get; set; }
        public int Estimation { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public TaskViewModel(Tasks task)
        {
            this.TaskId = task.TaskId;
            this.TaskName = task.TaskName;
            this.Description = task.Description;
            this.isActive = task.IsActive;
            this.Status = task.Status;
            this.Estimation = task.Estimation;
            if(task.StartTime != null)
                this.start = (DateTime) task.StartTime;
            if(task.EndTime != null)
                this.end = (DateTime)task.EndTime;
        }

    }
}