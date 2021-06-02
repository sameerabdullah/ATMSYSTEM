using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace BLL
{
    public class BL //Class Implementing Business Logic for ATM System
    {
        List<User> users; //List of User Objects
        List<Account> accs; //List of User Objects
        List<Transaction> trans; //List of User Transaction
        public BL() //Constructor of Business Logic Class, saving data from files to lists
        {
            users = (new AccessUsers()).getUsers();
            accs = (new AccessAccounts()).getAccounts();
            trans = (new AccessReport()).getReport();
        }

        public void saveModifications() //Function saving data from lists to files
        {
            (new AccessUsers()).setUsers(users);
            (new AccessAccounts()).setAccounts(accs);
            (new AccessReport()).setReport(trans);
        }
        public string checkLoginCreds(string login, string pin, ref int tries, string lastLogin) //Function validating Login feature
        {
            for (int i = 0; i < users.Count; i++)
                if (users[i].Login.Equals(login) && users[i].Pin.Equals(pin))
                {
                    if (users[i].Type.Equals("Admin"))
                        return "Admin";
                    else if (users[i].Type.Equals("Customer"))
                        for (int j = 0; j < accs.Count; j++)
                            if (accs[j].UserId == users[i].Id) {
                                if (accs[j].Status.Equals("Active")) return users[i].Type;
                                return "Disabled";
                            }
                }
            if (login.Equals(lastLogin)) tries--;
            else tries = 2;
            return "";
        }
        public void disableAccount(string login) //Function disabling account
        {
            for (int i = 0; i < users.Count; i++)
                if (users[i].Login.Equals(login))
                {
                    if (users[i].Type.Equals("Customer"))
                        for (int j = 0; j < accs.Count; j++)
                            if (accs[j].UserId == users[i].Id)
                            {
                                accs[j].Status = "Disabled";
                                Console.WriteLine("Account Disabled!");
                            }
                }
        }
        private int searchUser(string login) //Function Searching user on the base of login
        {
            for (int i = 0; i < users.Count; i++)
                if (users[i].Login.Equals(login)) return i;
            return -1;
        }
        private Transaction addTransaction(decimal amount, int userInd, int accInd, string transType) //Function adding new transaction
        {
            Transaction trn = new Transaction
            {
                AccId = accs[accInd].Id,
                UserId = users[userInd].Id,
                Name = users[userInd].Name,
                Amount = amount,
                Date = DateTime.Today,
                TransType = transType
            };
            trans.Add(trn);
            return trn;
        }
        public Transaction withdrawAmount(decimal amount, string login, ref decimal curBal) //Function removing cash from customer's account
        {
            int userInd = searchUser(login);
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].UserId == users[userInd].Id)
                {
                    if (accs[i].Balance < amount) break;
                    curBal = accs[i].Balance -= amount;
                    return addTransaction(amount, userInd, i, "Cash Withdrawal");
                }
            return null;
        }
        public Transaction transferAmount(int id, decimal amount, string login, ref decimal curBal) //Function removing cash from one customer's account and adding it to other's account
        {
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].Id == id)
                    for (int j = 0; j < users.Count; j++)
                        if (users[j].Id == accs[i].UserId)
                            accs[i].Balance += amount;
            int userInd = searchUser(login);
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].UserId == users[userInd].Id)
                {
                    if (accs[i].Balance < amount) break;
                    curBal = accs[i].Balance -= amount;
                    return addTransaction(amount, userInd, i, "Cash Transfer");
                }
            return null;
        }
        public Transaction depositAmount(decimal amount, string login, ref decimal curBal) //Function adding cash to customer's account
        {
            int userInd = searchUser(login);
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].UserId == users[userInd].Id)
                {
                    curBal = accs[i].Balance += amount;
                    return addTransaction(amount, userInd, i, "Cash Deposit");
                }
            return null;
        }
        public Account getReceiptData(string login) //Function returning data for receipt
        {
            int userInd = searchUser(login);
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].UserId == users[userInd].Id)
                {
                    return accs[i];
                }
            return null;
        }
        public bool isUserAlreadyExist(string login) //Function checking is user already exists
        {
            for (int i = 0; i < users.Count; i++)
                if (users[i].Login.Equals(login)) return true;
            return false;
        }
        public int createNewCustomer(string login, string pin, string name, string type, decimal balance, string status) //Function creating new customer
        {
            users.Add(new User
            {
                Id = users.Count == 0?1:users[users.Count - 1].Id + 1,
                Name = name,
                Login = login,
                Pin = pin,
                Type = "Customer"
            });
            accs.Add(new Account
            {
                Id = accs.Count == 0 ? 1 : accs[accs.Count - 1].Id + 1,
                UserId = users[users.Count - 1].Id,
                Type = type,
                Balance = balance,
                Status = status
            }); ;
            return accs[accs.Count - 1].Id;
        }
        public bool deleteCustomer(int id) //Function deleteing a customer
        {
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].Id == id)
                    for (int j = 0; j < users.Count; j++)
                        if (users[j].Id == accs[i].UserId)
                        {
                            accs.RemoveAt(i);
                            users.RemoveAt(j);
                            return true;
                        }
            return false;
        }
        public string getHoldername(int id) //Function returning account holder's name
        {
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].Id == id)
                    for (int j = 0; j < users.Count; j++)
                        if (users[j].Id == accs[i].UserId)
                            return users[j].Name;
            return "";
        }
        public bool updateCustomer(string login, string pin, string name, string status, int id) //Function updating customer's account details
        {
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].Id == id)
                    for (int j = 0; j < users.Count; j++)
                        if (users[j].Id == accs[i].UserId)
                        {
                            if (login.Length > 0) users[j].Login = login;
                            if (pin.Length > 0) users[j].Pin = pin;
                            if (name.Length > 0) users[j].Name = name;
                            if (status.Length > 0) accs[i].Status = status;
                            return true;
                        }
            return false;
        }
        public ( List<User> us, List<Account> ac) searchCustomers(int id, int userId, string name, decimal balance, string status) //Function searching the customer's accounts details
        {
            List<User> usrs = new List<User>(users);
            List<Account> acs = new List<Account>(accs);
            if(id != -1)
                for (int i = 0; i < acs.Count; i++)
                    if (acs[i].Id != id)
                        for (int j = 0; j < usrs.Count; j++)
                            if (usrs[j].Id == acs[i].UserId)
                            {
                                acs.RemoveAt(i);
                                usrs.RemoveAt(j);
                            }
            if (userId != -1)
                for (int i = 0; i < acs.Count; i++)
                    if (acs[i].UserId != userId)
                        for (int j = 0; j < usrs.Count; j++)
                            if (usrs[j].Id == acs[i].UserId)
                            {
                                acs.RemoveAt(i);
                                usrs.RemoveAt(j);
                            }
            if (!name.Equals(""))
                for (int i = 0; i < usrs.Count; i++)
                    if (!usrs[i].Name.Equals(name))
                        for (int j = 0; j < acs.Count; j++)
                            if (acs[j].UserId == usrs[i].Id)
                            {
                                acs.RemoveAt(j);
                                usrs.RemoveAt(i);
                            }
            if (balance != -1M)
                for (int i = 0; i < acs.Count; i++)
                    if (acs[i].Balance != balance)
                        for (int j = 0; j < usrs.Count; j++)
                            if (usrs[j].Id == acs[i].UserId)
                            {
                                acs.RemoveAt(i);
                                usrs.RemoveAt(j);
                            }
            if (!status.Equals(""))
                for (int i = 0; i < acs.Count; i++)
                    if (!acs[i].Status.Equals(status))
                        for (int j = 0; j < usrs.Count; j++)
                            if (usrs[j].Id == acs[i].UserId)
                            {
                                acs.RemoveAt(i);
                                usrs.RemoveAt(j);
                            }
            return (usrs, acs);
        }
        public (List<User> us, List<Account> ac) getAccByBalance(decimal min, decimal max) //Function returing account details between the range of min and max balance inclusive
        {
            List<User> usrs = new List<User>();
            List<Account> acs = new List<Account>();
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].Balance >= min && accs[i].Balance <= max)
                    for (int j = 0; j < users.Count; j++)
                        if (users[j].Id == accs[i].UserId)
                        {
                            acs.Add(accs[i]);
                            usrs.Add(users[j]);
                        }
            return (usrs, acs);
        }
        public List<Transaction> getTransByDate(DateTime startDate, DateTime endDate) //Function returing transaction details between the range of start and end date inclusive
        {
            List<Transaction> trns = new List<Transaction>();
            for (int i = 0; i < trans.Count; i++)
                if (trans[i].Date.Date >= startDate && trans[i].Date.Date <= endDate)
                    trns.Add(trans[i]);
            return trns;
        }
        public Account getAccountById(int id, ref string nam) //Function returing account details by Id
        {
            for (int i = 0; i < accs.Count; i++)
                if (accs[i].Id == id)
                    for (int j = 0; j < users.Count; j++)
                        if (users[j].Id == accs[i].UserId)
                        {
                            nam = users[j].Name;
                            return accs[i];
                        }
            return null;
        }
    }
}
