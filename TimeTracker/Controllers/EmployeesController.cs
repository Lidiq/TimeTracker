using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracker.DAL;
using TimeTracker.Models;

namespace TimeTracker.Controllers
{
    public class EmployeesController : Controller
    {
        //
        // GET: /Employees/

        public ActionResult Index(string name)
        {
            if (name != null)
            {
                ShowUser(name);
            }

            List<Users> users = UsersDAL.GetUsers();
            List<UserViewModel> usersViewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                usersViewModel.Add(
                    new UserViewModel(user)
                );
            }
            List<int> NumberOfTasks = new List<int>();
            List<int> SpentHours = new List<int>();
            foreach (var user in users)
            {
                NumberOfTasks.Add(UsersDAL.GetUserNumberOfTasks(user.UserID));
                SpentHours.Add(UsersDAL.GetUserSpentTime(user.UserID));
            }
            ViewBag.NumberOfTasks = NumberOfTasks;
            ViewBag.SpentHours = SpentHours;

            return View(usersViewModel);
            
        }

        public void ShowUser(string name)
        {
            ViewBag.ShowEmployee = true;
            List<Tasks> tasks = TasksDAL.GetTasksByUser(name);
            ViewBag.Tasks = tasks;


            List<int> SpentHoursOnTask = new List<int>();
            foreach (var task in tasks)
            {
                SpentHoursOnTask.Add(UsersDAL.GetUserSpentTimeOnTask(UsersDAL.GetUserByUsername(name).UserID, task.TaskId));
            }
            ViewBag.SpentHoursOnTask = SpentHoursOnTask;
        }
    }
}
