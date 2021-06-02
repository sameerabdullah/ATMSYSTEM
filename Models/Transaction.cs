using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Transaction //Transaction Business Object
    {
        private int accId;
        private int userId;
        private string name;
        private decimal amount;
        private string transType;
        private DateTime date;
        public int AccId
        {
            get { return accId; }
            set { accId = value; }
        }
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public string TransType
        {
            get { return transType; }
            set { transType = value; }
        }
        public DateTime Date{
            get { return date; }
            set { date = value; }
        }
    }
}
