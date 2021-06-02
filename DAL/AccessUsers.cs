using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DAL
{
    public class AccessUsers //Class Implementing DAL for Users
    {
        private string endecryptLogin(string login) //Function for encrypting/decrypting login
        {
            StringBuilder str = new StringBuilder(login);
            for (int i = 0; i < login.Length; i++)
            {
                if (login[i] >= 'A' && login[i] <= 'Z')
                    str[i] = Convert.ToChar('Z' - login[i] + 'A');
                else if (login[i] >= 'a' && login[i] <= 'z')
                    str[i] = Convert.ToChar('z' - login[i] + 'a');
                else if(login[i] >= '0' && login[i] <= '9')
                    str[i] = Convert.ToChar('9' - login[i] + '0');
            }
            return str.ToString();
        }
        private string endecryptPin(string pin) //Function for encrypting/decrypting pin
        {
            StringBuilder str = new StringBuilder(pin);
            for (int i = 0; i < pin.Length; i++)
                str[i] = Convert.ToChar('9' - pin[i] + '0');
            return str.ToString();
        }
        public List<User> getUsers() //Function reading all Users from file
        {
            List<User> users = new List<User>();
            try
            {
                FileStream inp = new FileStream("Users.txt", FileMode.Open, FileAccess.Read);
                StreamReader sinp = new StreamReader(inp);
                string line = "";
                while ((line = sinp.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    users.Add(new User
                    {
                        Id = Convert.ToInt32(values[0]),
                        Name = values[1],
                        Login = endecryptLogin(values[2]),
                        Pin = endecryptPin(values[3]),
                        Type = values[4]
                    });
                }
                sinp.Close();
                inp.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }
        public void setUsers(List<User> users) //Function writing all Users in file
        {
            try
            {
                FileStream fout = new FileStream("Users.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sout = new StreamWriter(fout);
                for (int i = 0; i < users.Count; i++)
                    sout.WriteLine($"{users[i].Id},{users[i].Name},{endecryptLogin(users[i].Login)},{endecryptPin(users[i].Pin)},{users[i].Type}");
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
