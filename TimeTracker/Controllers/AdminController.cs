using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TimeTracker.Models;
using TimeTracker.DAL;

namespace TimeTracker.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User.Identity.Name == "admin")
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //GET: /Admin/Employee/Edit
        public ActionResult Employee(string name)
        {
            if (!(User.Identity.IsAuthenticated && User.Identity.Name == "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Operation = name;
            if (name == "Edit")
            {
                Edit();
            }
            else if (name == "Register")
            {
                Register();
            }
            return View();
        }


        //GET: /Admin/Employee/Edit
        public ActionResult Edit()
        {
            List<Users> users = UsersDAL.GetAllUsers();
            List<UserViewModel> usersViewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                usersViewModel.Add(
                    new UserViewModel(user)
                );
            }
            return View(usersViewModel);
        }


        /**
         * Edit or delete user
         */ 
        [HttpPost]
        public ActionResult EditUser()
        {
            string username = Request.Form["user"];
            if (Request.Form["edit"] != null)
            {
                Users user = UsersDAL.GetUserByUsername(username);
                aspnet_Membership membershipUser = UsersDAL.GetMembershipUserByUsername(username);
                ViewBag.User = user; 
                ViewBag.Membership = membershipUser;
                ViewBag.Operation = "Edit";
            }
            else if (Request.Form["delete"] != null)
            {
                if (username != null)
                {
                    UsersDAL.DeleteUser(username);
                }
            }
            else if (Request.Form["save"] != null)
            {
                UsersDAL.EditUser(Request.Form["username"], Request.Form["firstname"], Request.Form["lastname"], Request.Form["email"], Request.Form["password"]);
            }
            return View("../Admin/Employee");
        }


        //
        // GET: /Admin/Employee/Register
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated && User.Identity.Name != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: /Admin/Register
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);
                UsersDAL.CreateUser(model.UserName, model.FirstName, model.LastName, model.Position);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion





        //GET: /Admin/Task
        public ActionResult Task(string name)
        {
            if (!(User.Identity.IsAuthenticated && User.Identity.Name == "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Operation = name;
            if (name == "Edit")
            {
                List<Tasks> tasks = TasksDAL.GetAllTasks();
                List<TaskViewModel> tasksViewModel = new List<TaskViewModel>();

                foreach (var task in tasks)
                {
                    tasksViewModel.Add(
                        new TaskViewModel(task)
                    );
                }
                return View(tasksViewModel);
            }
            else
            {
                List<Users> users = UsersDAL.GetAllUsers();
                List<UserViewModel> usersViewModel = new List<UserViewModel>();

                foreach (var user in users)
                {
                    usersViewModel.Add(
                        new UserViewModel(user)
                    );
                }
                return View(usersViewModel);
            }
            
        }


        // Edit a task
        public ActionResult EditTask()
        {
            if (!(User.Identity.IsAuthenticated && User.Identity.Name == "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            Guid taskId = Guid.Parse(Request.Form["taskId"]);
            if (Request.Form["edit"] != null)
            {
                Tasks task = TasksDAL.GetTaskById(taskId);
                ViewBag.Task = task;
                ViewBag.Operation = "Edit";
            }
            else if (Request.Form["delete"] != null)
            {
                if (taskId != null)
                {
                    TasksDAL.DeleteTask(taskId);
                }
            }
            else if (Request.Form["save"] != null)
            {
                TasksDAL.EditTask(Guid.Parse(Request.Form["taskId"]), Request.Form["taskName"], Request.Form["description"],
                                  DateTime.Parse(Request.Form["start"]), DateTime.Parse(Request.Form["end"]), 
                                  int.Parse(Request.Form["estimation"]), Request.Form["status"]);
            }
            return View("../Admin/Task");


        }


        // Create a task
        public ActionResult CreateTask()
        {
            if (!(User.Identity.IsAuthenticated && User.Identity.Name == "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                if (Request.Form["save"] != null)
                {
                    DateTime start = DateTime.Now;
                    if (Request.Form["start"] != "")
                    {
                        start = DateTime.Parse(Request.Form["start"]);
                    }
                    DateTime end = DateTime.Now;
                    if (Request.Form["end"] != "")
                    {
                        end = DateTime.Parse(Request.Form["end"]);
                    }
                    string values = Request.Form["users"];
                    string[] userIds = values.Split(',');

                    TasksDAL.CreateTask(Request.Form["taskName"], Request.Form["description"], start,
                       end, int.Parse(Request.Form["estimation"]), Request.Form["status"], userIds);
                }
                return RedirectToAction("Task", "Admin");
            }
            catch (Exception ex)
            {
                return View();
            }
        }




        //GET: /Admin/Card
        public ActionResult Card(string name)
        {
            if (!(User.Identity.IsAuthenticated && User.Identity.Name == "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Operation = name;
            if (name == "Edit")
            {
                string username = Request.Form["username"];
                List<WorkCards> cards = CardsDAL.GetCardsByUser(username);
                List<CardViewModel> cardsViewModel = new List<CardViewModel>();
                foreach (var card in cards)
                {
                    cardsViewModel.Add(
                        new CardViewModel(card)
                    );
                }
                return View(cardsViewModel);
                
            }
            List<Users> users = UsersDAL.GetAllUsers();
            List<UserViewModel> usersViewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                usersViewModel.Add(
                    new UserViewModel(user)
                );
            }
            return View(usersViewModel);
        }




        public ActionResult EditCard()
        {
            if (!(User.Identity.IsAuthenticated && User.Identity.Name == "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            Guid cardId = Guid.Parse(Request.Form["cardId"]);
            if (Request.Form["edit"] != null)
            {
                WorkCards card = CardsDAL.GetCardById(cardId);
                ViewBag.Card = card;
                ViewBag.Operation = "Edit";
                return View("Card");
            }
            else if (Request.Form["delete"] != null)
            {
                if (cardId != null)
                {
                    CardsDAL.DeleteCard(cardId);
                }
                ViewBag.Delete = "Successfully deleted the card";
            }
            else if (Request.Form["save"] != null)
            {
                CardsDAL.EditCard(Guid.Parse(Request.Form["cardId"]), DateTime.Parse(Request.Form["start"]),
                                  int.Parse(Request.Form["duration"]), Request.Form["comment"]);
                ViewBag.Edit = "Successfully edited the card";
            }
            return View();
        }

        
    }
}
