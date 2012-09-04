using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracker.Models;
using TimeTracker.DAL;

namespace TimeTracker.Controllers
{
    public class TasksController : Controller
    {
        //
        // GET: /Tasks/

        public ActionResult Index(string id)
        {
            if (id != null)
            {
                ShowTask(Guid.Parse(id));
            }

            List<Tasks> tasks = TasksDAL.GetTasks();
            List<TaskViewModel> tasksViewModel = new List<TaskViewModel>();

            foreach (var task in tasks)
            {
                tasksViewModel.Add(
                    new TaskViewModel(task)
                );
            }
            return View(tasksViewModel);
        }


        /**show single task**/
        public void ShowTask(Guid taskId)
        {
            ViewBag.ShowTask = true;
            List<Users> employees = UsersDAL.GetUsersByTaskId(taskId);
            ViewBag.Employees = employees;

            List<int> SpentHours = new List<int>();
            foreach (var employee in employees)
            {
                SpentHours.Add(UsersDAL.GetUserSpentTimeOnTask(employee.UserID, taskId));
            }
            ViewBag.SpentHoursOnTask = SpentHours;
        }
    }
}
