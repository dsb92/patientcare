using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatientCareAdmin.Models;

namespace PatientCareAdmin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && model.UserName == "test" && model.Password == "1234")
            {
                Session["User"] = model.UserName;
            }
            else
                ModelState.AddModelError("", "The Username or password provided is incorrect.");

            return RedirectToAction("Index", "Home");
        }
    }
}

