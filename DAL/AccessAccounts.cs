using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace DAL
{
    public class AccessAccounts //Class Implementing DAL for Accounts
    {
        public List<Account> getAccounts() //Function reading all Accounts from file
        {
            List<Account> accs = new List<Account>();
            try
            {
                FileStream inp = new FileStream("Accounts.txt", FileMode.Open, FileAccess.Read);
                StreamReader sinp = new StreamReader(inp);
                string line = "";
                while ((line = sinp.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    accs.Add(new Account
                    {
                        Id = Convert.ToInt32(values[0]),
                        UserId = Convert.ToInt32(values[1]),
                        Type = values[2],
                        Balance = Convert.ToDecimal(values[3]),
                        Status = values[4]
                    });
                }
                sinp.Close();
                inp.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return accs;
        }
        public void setAccounts(List<Account> accs) //Function writing all Accounts in file
        {
            try
            {
                FileStream fout = new FileStream("Accounts.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sout = new StreamWriter(fout);
                for (int i = 0; i<accs.Count; i++)
                    sout.WriteLine($"{accs[i].Id},{accs[i].UserId},{accs[i].Type},{accs[i].Balance},{accs[i].Status}");
                sout.Close();
                fout.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
