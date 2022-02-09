using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankingSystem.Models;

namespace BankingSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private BankingEntities3 db = new BankingEntities3();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "username,password")] Employee employee)
        {
            List<Employee> emp = db.Employees.Where(e => e.username == employee.username && e.password == employee.password).ToList();
            if (emp.Count == 0)
            {
                return View();
            }
            else
            {
                Session["username"] = employee.username;
                Session["role"] = "employee";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Search()
        {
            List<Customer> customers = new List<Customer>();
            ViewBag.Customers = customers;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "FirstName,LastName")] Customer customer)
        {
            List<Customer> customers = db.Customers.Where(c => c.FirstName == customer.FirstName && c.LastName == customer.LastName).ToList();
            ViewBag.Customers = customers;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
