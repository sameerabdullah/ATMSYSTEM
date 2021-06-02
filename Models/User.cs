using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace Models
{
    public class User //User Business Object
    {
        private int id;
        private string name;
        private string login;
        private string pin;
        private string type;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        public string Pin
        {
            get { return pin; }
            set { pin = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}

