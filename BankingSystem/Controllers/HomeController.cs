using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankingSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                if (Session["role"].ToString() == "customer")
                    return RedirectToAction("Index", "Customers");
                else
                    return RedirectToAction("Index", "Employees");
            }
            else
                return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("username");
            Session.Remove("role");
            return RedirectToAction("Index");

        }
    }
}