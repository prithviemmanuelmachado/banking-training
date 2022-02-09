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
    public class AccountsController : Controller
    {
        private BankingEntities3 db = new BankingEntities3();

        // GET: Accounts
        public ActionResult Index()
        {
            int id = Int32.Parse(Session["username"].ToString());
            var accounts = db.Accounts.Where(a => a.CustomerID == id);
            return View(accounts.ToList());
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Balance")] Account account)
        {
            account.CustomerID = Int32.Parse(Session["CustID"].ToString());
            db.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Details", "Customers");
        }

        public ActionResult Statement(int id)
        {
            Session["AccountID"] = id;
            return RedirectToAction("Index", "Transactions");
        }

        public ActionResult Transact(int id)
        {
            Session["AccountID"] = id;
            return RedirectToAction("Create", "Transactions");
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
