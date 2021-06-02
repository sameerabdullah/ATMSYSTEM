using System;
using System.Collections.Generic;
using System.Text;

namespace Models //Class implementinging Business Objects
{
    public class Account //Account Business Object
    {
        private int id;
        private int userId;
        private string type;
        private decimal balance;
        private string status;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public decimal Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
