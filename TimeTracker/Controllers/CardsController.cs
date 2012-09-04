using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracker.DAL;
using TimeTracker.Models;

namespace TimeTracker.Controllers
{
    public class CardsController : Controller
    {
        //
        // GET: /Cards/

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Tasks> tasks = TasksDAL.GetTasksByUser(User.Identity.Name);
                List<TaskViewModel> tasksViewModel = new List<TaskViewModel>();

                foreach (var task in tasks)
                {
                    tasksViewModel.Add(
                        new TaskViewModel(task)
                    );
                }
                return View(tasksViewModel);
            }
            return RedirectToAction("Index", "Home");
        }

       
        //
        // POST: /Cards/Create

        [HttpPost]
        public ActionResult CreateCard()
        {
            try
            {
                Guid userId = UsersDAL.GetUserByUsername(User.Identity.Name).UserID;
                if (Request.Form["save"] != null)
                {
                    var test = Request.Form["task"];
                    Guid taskId = Guid.Parse(Request.Form["task"]);  
                    CardsDAL.CreateCard(taskId, userId, DateTime.Now, 
                       int.Parse(Request.Form["duration"]), Request.Form["comment"]);
                }
                ViewBag.Success = "Your work card was safed";
                return View();
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

    }
}
