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
    public class TransactionsController : Controller
    {
        private BankingEntities3 db = new BankingEntities3();

        // GET: Transactions
        public ActionResult Index()
        {
            
            List<Transaction> transactions;
            if (Session["AccountID"] != null)
            {
                int accID = Int32.Parse(Session["AccountID"].ToString());
                transactions = db.Transactions.Where(t => t.FromAccountNumber == accID).ToList();
            }
            else
            {
                int custID = Int32.Parse(Session["username"].ToString());
                transactions = db.Transactions.Where(t => t.Account.CustomerID == custID).ToList();
            }
            return View(transactions);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            Transaction transaction = new Transaction();
            transaction.FromAccountNumber = int.Parse(Session["AccountID"].ToString());
            return View(transaction);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FromAccountNumber,ToAccountNumber,ToBankName, Amount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                Account acc = db.Accounts.Where(a => a.ID == transaction.FromAccountNumber).FirstOrDefault();
                acc.Balance -= transaction.Amount;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.FromAccountNumber = new SelectList(db.Accounts, "ID", "ID", transaction.FromAccountNumber);
            return View(transaction);
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
