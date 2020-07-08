using Pro_Salles.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pro_Salles.Class
{
    public static class Master_Finance
    {
        ///////74
        public static AccountBalance GetAccountBalance(int accountid)
        {
            using (var db = new Pro_SallesDataContext())
            {
                var query = db.Journals.Where(x => x.Account_ID == accountid).Select(x => new { x.Debit, x.Credit });
                var TotalCredit = query.Sum(x => (double?)x.Credit) ?? 0;
                var TotalDebit = query.Sum(x => (double?)x.Debit) ?? 0;
                var account = db.Accounts.Single(x => x.ID == accountid);
                var b = new AccountBalance(accountid, account.name, Math.Abs(TotalCredit - TotalDebit),
                    (TotalCredit > TotalDebit) ? AccountBalance.BalanceTypes.Credit : AccountBalance.BalanceTypes.Debit);
                return b;
            }
        }


        public static AccountBalance GetAccountBalance(this Account account, int accountid)
        {
            return GetAccountBalance(account.ID);
        }

        public class AccountBalance
        {
            public AccountBalance(int id, string name, double amount, BalanceTypes balanceTypes)
            {
                ID = id;
                Name = name;
                BalanceAmount = amount;
                BalanceType = balanceTypes;
            }

            public int ID { get; }
            public string Name { get; }
            public double BalanceAmount { get; }
            public BalanceTypes BalanceType { get; }
            public string Balance
            {
                get
                {
                    return BalanceAmount.ToString() + " " + ((BalanceType == BalanceTypes.Credit) ? "دائن" : "مدين");
                }
            }

            public enum BalanceTypes
            {
                Credit = 1,
                Debit
            }
        }
    }
}