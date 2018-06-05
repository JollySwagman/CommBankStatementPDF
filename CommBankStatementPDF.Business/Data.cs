using CommBankStatementPDF.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace CommBankStatementPDF.Business
{
    public class Data
    {
        public static void DeleteAll()
        {
            using (var db = new CommBankStatementPDF.Data.CBAStatementsEntities())
            {
                db.Transactions.RemoveRange(db.Transactions);
                db.SaveChanges();
            }
        }

        public static void Save(IList<Transaction> transactions)
        {
            try
            {
                using (var db = new CommBankStatementPDF.Data.CBAStatementsEntities())
                {
                    var customers = db.Set<Transactions>();

                    foreach (var item in transactions)
                    {
                        customers.Add(new Transactions
                        {
                            Source = item.Source[0],
                            AccountType = item.Type.ToString(),
                            Amount = item.Amount,
                            Biller = item.Biller,
                            Date = item.Date
                        });
                    }

                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Trace.WriteLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                throw;
            }
        }
    }
}