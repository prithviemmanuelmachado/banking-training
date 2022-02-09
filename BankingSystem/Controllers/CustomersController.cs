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
    public class CustomersController : Controller
    {
        private BankingEntities3 db = new BankingEntities3();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details()
        {
            int id = Int32.Parse(Session["CustID"].ToString());
            Customer customer = db.Customers.Find(id);
            ViewBag.Accounts = db.Accounts.Where(a => a.CustomerID == id).ToList();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Address,Gender,DOB,ContactNumber,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                customer.Password = new string(Enumerable.Repeat(chars, chars.Length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Employees");
            }

            return View(customer);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "ID,Password")] Customer customer)
        {
            List<Customer> cust = db.Customers.Where(e => e.ID == customer.ID && e.Password == customer.Password).ToList();
            if (cust.Count == 0)
            {
                return View();
            }
            else
            {
                Session["username"] = customer.ID;
                Session["role"] = "customer";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Statement()
        {
            Session.Remove("AccountID");
            return RedirectToAction("Index", "Transactions");
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
