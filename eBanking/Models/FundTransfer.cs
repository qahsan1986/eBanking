using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBanking.Models
{
    public class FundTransfer
    {
        private eBankingEntities db = new eBankingEntities();
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }

        public Decimal Amount { get; set; }
        public void SaveTrasaction( FundTransfer eFund)
        {
            Account fromAccount = db.Accounts.Where(t => t.AccountID == eFund.FromAccount).FirstOrDefault();
            Account toAccount = db.Accounts.Where(t => t.AccountID == eFund.ToAccount).FirstOrDefault();
            Transaction transaction = new Transaction();
        
            transaction.TransactionDate = System.DateTime.Now;
            transaction.FromAccountID = eFund.FromAccount;
            transaction.ToAccountID = eFund.ToAccount;
            transaction.TransactionAmount = eFund.Amount;
            transaction.FromBalance = fromAccount.Balance - eFund.Amount;
            transaction.ToBalance = toAccount.Balance + eFund.Amount;
            db.Transactions.Add(transaction);
            db.SaveChanges();

            updateAccountBalance(fromAccount, transaction.FromBalance,db);
            updateAccountBalance(toAccount, transaction.ToBalance,db);
        }
        public void updateAccountBalance(Account account,decimal balanceAmount, eBankingEntities db) {
            account.Balance = balanceAmount;
            db.SaveChanges();
        }
    }
}