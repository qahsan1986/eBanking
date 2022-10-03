using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eBanking;
using eBanking.Models;

namespace eBanking.Controllers
{
    public class FundTransferController : Controller
    {
        private eBankingEntities db = new eBankingEntities();
        public FundTransfer fundTransfer = new FundTransfer();
        public ActionResult FundTransfer()
        {
            ViewBag.FromAccount = new SelectList(db.Accounts, "AccountID", "AccountName");
            ViewBag.ToAccount = new SelectList(db.Accounts, "AccountID", "AccountName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FundTransfer(eBanking.Models.FundTransfer eFund)
        {
   
            if (ModelState.IsValid)
            {
                eBanking.Models.Account fromAccount = db.Accounts.Where(t => t.AccountID == eFund.FromAccount).FirstOrDefault();
                eBanking.Models.Account toAccount = db.Accounts.Where(t => t.AccountID == eFund.ToAccount).FirstOrDefault();

                if (eFund.Amount > fromAccount.Balance)
                {
                    ViewBag.FromAccount = new SelectList(db.Accounts, "AccountID", "AccountName");
                    ViewBag.ToAccount = new SelectList(db.Accounts, "AccountID", "AccountName");

                    ModelState.AddModelError("FromAccount", "Balance is insufficent. Your current balace is " + fromAccount.Balance.ToString("N2")); ;
                }
                else
                {
                    if (fromAccount != null && toAccount != null)
                    {
                        fundTransfer.SaveTrasaction(eFund);
                       
                    }
                    else
                    {
                        ModelState.AddModelError("Account", "Account does not exists");
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(eFund);
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
