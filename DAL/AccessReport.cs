using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DAL
{
    public class AccessReport //Class Implementing DAL for Transactions
    {
        public List<Transaction> getReport() //Function reading all Transactions from file
        {
            List<Transaction> trans = new List<Transaction>();
            try
            {
                FileStream inp = new FileStream("Transactions.txt", FileMode.Open, FileAccess.Read);
                StreamReader sinp = new StreamReader(inp);
                string line = "";
                while ((line = sinp.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    trans.Add(new Transaction
                    {
                        AccId = Convert.ToInt32(values[0]),
                        UserId = Convert.ToInt32(values[1]),
                        Name = values[2],
                        Amount = Convert.ToDecimal(values[3]),
                        Date = Convert.ToDateTime(values[4]),
                        TransType = values[5]
                    });
                }
                sinp.Close();
                inp.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return trans;
        }
        public void setReport(List<Transaction> trans) //Function writing all Transactions in file
        {
            try
            {
                FileStream fout = new FileStream("Transactions.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sout = new StreamWriter(fout);
                for (int i = 0; i < trans.Count; i++)
                    sout.WriteLine($"{trans[i].AccId},{trans[i].UserId},{trans[i].Name},{trans[i].Amount},{trans[i].Date},{trans[i].TransType}");
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
