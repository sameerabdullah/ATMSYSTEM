using BLL;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Views
{
    public class View //Class Implementing View for ATM System
    {
        BL bLogic; //Business Logic Object
        public View() //Constructor of View Class
        {
            bLogic = new BL();
        }
        public void MainView() //Main Function of Handing all views
        {
            int tries = 3;
            string login = "";
            string lastLogin = "";
            while (tries > 0)
            {
                Console.Write("Enter Login: ");
                Console.ForegroundColor = ConsoleColor.Green;
                login = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Enter Pin code: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string pin = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                switch (bLogic.checkLoginCreds(login, pin, ref tries, lastLogin))
                {
                    case "Admin":
                        adminSwitch((char)AdminMainMenu());
                        break;
                    case "Customer":
                        customerSwitch(CustomerMainMenu(), login);
                        break;
                    case "Disabled":
                        Console.WriteLine("Account was disabled!");
                        break;
                    default:
                        lastLogin = login;
                        Console.WriteLine($"Invalid Login or Pin! {tries} tries left");
                        continue;
                }
                break;
            }
            if (tries <= 0) bLogic.disableAccount(login);
            bLogic.saveModifications();
            MainView();
        }
        private int AdminMainMenu() //Function returning admin's mode decision
        {
            Console.WriteLine("         =========== WELCOME ADMIN ===========");
            Console.WriteLine("1----Create New Account");
            Console.WriteLine("2----Delete Existing Account");
            Console.WriteLine("3----Update Account Information");
            Console.WriteLine("4----Search for Account");
            Console.WriteLine("5----View Reports");
            Console.WriteLine("6----Exit");
            char option;
            while (true)
            {
                Console.Write("Please select one of the above options: ");
                Console.ForegroundColor = ConsoleColor.Green;
                option = Console.ReadKey().KeyChar;
                Console.ForegroundColor = ConsoleColor.White;
                if (option < '1' || option > '6')
                {
                    Console.WriteLine("\nInvalid Input!");
                    continue;
                }
                break;
            }
            return option;
        }
        private char CustomerMainMenu() //Function returning customer's mode decision
        {
            Console.WriteLine("         =========== WELCOME Customer ===========");
            Console.WriteLine("1----Withdraw Cash");
            Console.WriteLine("2----Cash Transfer");
            Console.WriteLine("3----Deposit Cash");
            Console.WriteLine("4----Display Balance");
            Console.WriteLine("5----Exit");
            char option;
            while (true)
            {
                Console.Write("Please select one of the above options: ");
                Console.ForegroundColor = ConsoleColor.Green;
                option = Console.ReadKey().KeyChar;
                Console.ForegroundColor = ConsoleColor.White;
                if (option < '1' || option > '5')
                {
                    Console.WriteLine("\nInvalid Input!");
                    continue;
                }
                break;
            }
            return option;
        }
        private decimal fastWithdrawAmount(char opt) //Function returning withdrawal amount on the decision of customer's option
        {
            switch (opt)
            {
                case '1':
                    return 500M;
                case '2':
                    return 1000M;
                case '3':
                    return 2000M;
                case '4':
                    return 5000M;
                case '5':
                    return 10000M;
                case '6':
                    return 15000M;
                case '7':
                    return 20000M;
            }
            return 0M;
        }
        private char withdrawCash(string login)  //Function performing customer's Cash Withdrawal feature
        {
            Console.WriteLine("         =========== WITHDRAW CASH ===========");
            Console.WriteLine("a) Fast Cash");
            Console.WriteLine("b) Normal Cash");
            char option;
            while (true)
            {
                Console.Write("Please select one of the above options: ");
                Console.ForegroundColor = ConsoleColor.Green;
                option = Console.ReadKey().KeyChar;
                Console.ForegroundColor = ConsoleColor.White;
                if (option != 'a' && option != 'b')
                {
                    Console.WriteLine("\nInvalid Input!");
                    continue;
                }
                break;
            }
            Console.Clear();
            decimal amount = 0M;
            switch (option)
            {
                case 'a':
                    Console.WriteLine("         =========== FAST WITHDRAW ===========");
                    Console.WriteLine("1----500");
                    Console.WriteLine("2----1000");
                    Console.WriteLine("3----2000");
                    Console.WriteLine("4----5000");
                    Console.WriteLine("5----10000");
                    Console.WriteLine("6----15000");
                    Console.WriteLine("7----20000");
                    char opt;
                    while (true)
                    {
                        Console.Write("Select one of the denominations of money: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        opt = Console.ReadKey().KeyChar;
                        Console.ForegroundColor = ConsoleColor.White;
                        if (opt < '1' || opt > '8')
                        {
                            Console.WriteLine("\nInvalid Input!");
                            continue;
                        }
                        break;
                    }
                    amount = fastWithdrawAmount(opt);
                    while (true)
                    {
                        Console.Write($"\nAre you sure you want to withdraw Rs.{amount} (Y/N)? ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        char yn = Console.ReadKey().KeyChar;
                        Console.ForegroundColor = ConsoleColor.White;
                        if (yn != 'Y' && yn != 'N')
                        {
                            Console.WriteLine("\nInvalid Input!");
                            continue;
                        }
                        if (yn == 'Y')
                        {
                            if (!commonWithdrawCode(amount, login)) break;
                        }
                        break;
                    }
                    break;
                case 'b':
                    while (true)
                        try
                        {
                            Console.Write("Enter the withdrawal amount: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            amount = Convert.ToDecimal(Console.ReadLine());
                            Console.ForegroundColor = ConsoleColor.White;
                            if (amount % 500 != 0)
                            {
                                Console.WriteLine("Amount is not multiple of 500!");
                                continue;
                            }
                            break;
                        }
                        catch (Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Invalid Input!");
                        }
                    commonWithdrawCode(amount, login);
                    break;
            }
            return option;
        }
        private void customerSwitch(char opt, string login) //Function making decision of customer's modes choice
        {
            Console.Clear();
            switch (opt)
            {
                case '1':
                    withdrawCash(login);
                    break;
                case '2':
                    transferCash(login);
                    break;
                case '3':
                    depositCash(login);
                    break;
                case '4':
                    displayReceipt(login);
                    break;
                case '5':
                    Environment.Exit(0);
                    break;
            }
            bLogic.saveModifications();
            Console.WriteLine("\nPress any key to go to main menu!");
            Console.ReadKey();
            Console.Clear();
            MainView();
        }
        private void adminSwitch(char opt) //Function making decision of admin's modes choice
        {
            Console.Clear();
            switch (opt)
            {
                case '1':
                    createAccount();
                    break;
                case '2':
                    deleteAccount();
                    break;
                case '3':
                    updateAccount();
                    break;
                case '4':
                    searchAccount();
                    break;
                case '5':
                    viewReport();
                    break;
                case '6':
                    Environment.Exit(0);
                    break;
            }
            bLogic.saveModifications();
            Console.WriteLine("\nPress any key to go to main menu!");
            Console.ReadKey();
            Console.Clear();
            MainView();
        }
        private void printWithDrawalReceipt(Transaction transaction, decimal curBal) //Function prints receipt of Cash Withdrawl done by an account
        {
            Console.WriteLine($"\n\nAccount #{transaction.AccId}");
            Console.WriteLine($"{transaction.Date.ToString("dd/MM/yyyy")}\n");
            Console.WriteLine($"Withdrawn: {transaction.Amount}");
            Console.WriteLine($"Balance: {curBal}");
        }
        private void printDepositReceipt(Transaction transaction, decimal curBal) //Function prints receipt of Cash Deposit done by an account
        {
            Console.WriteLine($"\n\nAccount #{transaction.AccId}");
            Console.WriteLine($"{transaction.Date.ToString("dd/MM/yyyy")}\n");
            Console.WriteLine($"Deposited: {transaction.Amount}");
            Console.WriteLine($"Balance: {curBal}");
        }
        private void printTransferReceipt(Transaction transaction, decimal curBal) //Function prints receipt of Cash Transfer done by an account
        {
            Console.WriteLine($"\n\nAccount #{transaction.AccId}");
            Console.WriteLine($"{transaction.Date.ToString("dd/MM/yyyy")}\n");
            Console.WriteLine($"Amount Transferred:: {transaction.Amount}");
            Console.WriteLine($"Balance: {curBal}");
        }
        private bool commonWithdrawCode(decimal amount, string login) //Function performs common functionality used in customer's Cash Withdrawal feature
        {
            decimal curBal = 0M;
            Transaction transaction = null;
            if ((transaction = bLogic.withdrawAmount(amount, login, ref curBal)) != null)
            {
                Console.WriteLine("\nCash Successfully Withdrawn!");
                while (true)
                {
                    Console.Write($"Do you wish to print a receipt (Y/N)? ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    char res = Console.ReadKey().KeyChar;
                    Console.ForegroundColor = ConsoleColor.White;
                    if (res != 'Y' && res != 'N')
                    {
                        Console.WriteLine("\nInvalid Input!");
                        continue;
                    }
                    if (res == 'Y')
                    {
                        printWithDrawalReceipt(transaction, curBal);
                    }
                    return false;
                }
            }
            else
                Console.WriteLine("\nBalance is not sufficent!");
            return true;
        }
        private void transferCash(string login) //Function working on customer's Transfer Cash feature
        {
            decimal amount;
            while (true)
                try
                {
                    Console.Write("Enter amount in multiples of 500: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    amount = Convert.ToDecimal(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    if (amount % 500 != 0)
                    {
                        Console.WriteLine("Amount is not multiple of 500!");
                        continue;
                    }
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            int id;
            while (true)
                try
                {
                    Console.Write("Enter the account number to which you want to transfer: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    id = Convert.ToInt32(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            string name = bLogic.getHoldername(id);
            if (bLogic.getHoldername(id) != "")
            {
                int nid;
                while (true)
                    try
                    {
                        Console.Write($"You wish to deposit Rs {amount} in account held by {name}; If this information is correct please re-enter the account number: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        nid = Convert.ToInt32(Console.ReadLine());
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Invalid Input!");
                    }
                decimal curBal = 0M;
                if (id == nid)
                {
                    Transaction transaction = bLogic.transferAmount(id, amount, login, ref curBal);
                    if (transaction == null) 
                    {
                        Console.WriteLine("Transaction Failed");
                        return;
                    }
                    Console.WriteLine("Transaction Confirmed");
                    while (true)
                    {
                        Console.Write($"Do you wish to print a receipt (Y/N)? ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        char res = Console.ReadKey().KeyChar;
                        Console.ForegroundColor = ConsoleColor.White;
                        if (res != 'Y' && res != 'N')
                        {
                            Console.WriteLine("\nInvalid Input!");
                            continue;
                        }
                        if (res == 'Y')
                        {
                            printTransferReceipt(transaction, curBal);
                        }
                        return;
                    }
                }
                else Console.Write("ID mismatch!");
            }
        }
        private void depositCash(string login) //Function working on customer's Deposit Cash feature
        {
            decimal amount = 0M;
            while (true)
                try
                {
                    Console.Write("Enter the cash amount to deposit: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    amount = Convert.ToDecimal(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            decimal curBal = 0M;
            Transaction transaction = bLogic.depositAmount(amount, login, ref curBal);
            if (transaction == null)
            {
                Console.WriteLine("Transaction Failed");
                return;
            }
            Console.WriteLine("\nCash Successfully Deposit!");
            while (true)
            {
                Console.Write($"Do you wish to print a receipt (Y/N)? ");
                Console.ForegroundColor = ConsoleColor.Green;
                char res = Console.ReadKey().KeyChar;
                Console.ForegroundColor = ConsoleColor.White;
                if (res != 'Y' && res != 'N')
                {
                    Console.WriteLine("\nInvalid Input!");
                    continue;
                }
                if (res == 'Y')
                {
                    if(transaction != null)
                        printDepositReceipt(transaction, curBal);
                }
                break;
            }
        }
        private void displayReceipt(string login) //Function prints accounts receipts
        {
            Account acc = null;
            if ((acc = bLogic.getReceiptData(login)) != null)
            {
                Console.WriteLine($"Account #{acc.Id}");
                Console.WriteLine($"{DateTime.Today.ToString("dd/MM/yyyy")}\n");
                Console.WriteLine($"Balance: {acc.Balance}");
            }
            else Console.WriteLine("Process Failed!"); ;
        }
        private bool validatePin(string pin)
        {
            if (pin.Length != 5) return false;
            for (int i = 0; i < 5; i++)
                if (pin[i] < '0' || pin[i] > '9') return false;
            return true;
        }
        private void createAccount() //Function working on admin's Create Account feature
        {
            string login, pin, type, status;
            while (true)
            {
                Console.Write("Login: ");
                Console.ForegroundColor = ConsoleColor.Green;
                login = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (!bLogic.isUserAlreadyExist(login) && login.Length > 0) break;
                Console.WriteLine("User already exists or something else");
            }
            while (true)
            {
                Console.Write("Enter Pin code: ");
                Console.ForegroundColor = ConsoleColor.Green;
                pin = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (validatePin(pin)) break;
                Console.WriteLine("Invalid! Please enter 5 digits");
            }
            Console.Write("Holder's Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write("Type (Savings,Current): ");
                Console.ForegroundColor = ConsoleColor.Green;
                type = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (type.Equals("Savings") || type.Equals("Current")) break;
                Console.WriteLine("Invalid! Please enter Savings or Current");
            }
            decimal balance;
            while (true)
                try
                {
                    Console.Write("Starting Balance: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    balance = Convert.ToDecimal(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            while (true)
            {
                Console.Write("Status: ");
                Console.ForegroundColor = ConsoleColor.Green;
                status = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (status.Equals("Active") || status.Equals("Disabled")) break;
                Console.WriteLine("Invalid! Please enter Active or Disabled");
            }
            Console.WriteLine($"Account Successfully Created – the account number assigned is: {bLogic.createNewCustomer(login, pin, name, type, balance, status)}");
        }
        private void deleteAccount() //Function working on admin's Delete Account feature
        {
            int id;
            while (true)
                try
                {
                    Console.Write("Enter the account number to which you want to delete: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    id = Convert.ToInt32(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            string name = bLogic.getHoldername(id);
            if (bLogic.getHoldername(id) != "")
            {
                int nid;
                while (true)
                    try
                    {
                        Console.Write($"You wish to delete the account held by {name}; If this information is correct please re - enter the account number: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        nid = Convert.ToInt32(Console.ReadLine());
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Invalid Input!");
                    }
                if (id == nid)
                {
                    if (bLogic.deleteCustomer(id)) Console.WriteLine("Account Deleted Successfully");
                }
                else Console.Write("ID mismatch!");
            }
            else Console.Write("Account does not exist!");
        }
        private void updateAccount() //Function working on admin's Update Account feature
        {
            int id;
            while (true)
                try
                {
                    Console.Write("Enter the Account Number: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    id = Convert.ToInt32(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            string nam = "";
            Account accDet = bLogic.getAccountById(id, ref nam);
            Console.WriteLine($"\nAccount # {accDet.Id}");
            Console.WriteLine($"Type: {accDet.Type}");
            Console.WriteLine($"Holder: {nam}");
            Console.WriteLine($"Balance: {accDet.Balance}");
            Console.WriteLine($"Status: {accDet.Status}\n");
            string login, pin, status;
            while (true)
            {
                Console.Write("Login: ");
                Console.ForegroundColor = ConsoleColor.Green;
                login = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (!bLogic.isUserAlreadyExist(login)) break;
                Console.WriteLine("User already exists or or something else");
            }
            while (true)
            {
                Console.Write("Enter Pin code: ");
                Console.ForegroundColor = ConsoleColor.Green;
                pin = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (validatePin(pin) || pin.Equals("")) break;
                Console.WriteLine("Invalid! Please enter 5 digits");
            }
            Console.Write("Holder's Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write("Status: ");
                Console.ForegroundColor = ConsoleColor.Green;
                status = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (status.Equals("Active") || status.Equals("Disabled") || status.Equals("")) break;
                Console.WriteLine("Invalid! Please enter Active or Disabled");
            }
            if (bLogic.updateCustomer(login, pin, name, status, id)) Console.WriteLine("Your account has been successfully updated");
            else Console.WriteLine("Error updating account!");
        }
        private void searchAccount() //Function working on admin's Searc Accounts feature
        {
            Console.WriteLine("SEARCH MENU:");
            int id = -1;
            while (true)
                try
                {
                    Console.Write("Account ID: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string sid;
                    if ((sid = Console.ReadLine()).Length != 0) id = Convert.ToInt32(sid);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            int userId = -1;
            while (true)
                try
                {
                    Console.Write("User ID: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string sid;
                    if ((sid = Console.ReadLine()).Length != 0) userId = Convert.ToInt32(sid);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            Console.Write("Holder's Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            string type;
            while (true)
            {
                Console.Write("Type (Savings,Current): ");
                Console.ForegroundColor = ConsoleColor.Green;
                type = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (type.Equals("Savings") || type.Equals("Current") || type.Equals("")) break;
                Console.WriteLine("Invalid! Please enter Savings or Current");
            }
            decimal balance = -1M;
            while (true)
                try
                {
                    Console.Write("Balance: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string sb;
                    if ((sb = Console.ReadLine()).Length != 0) balance = Convert.ToInt32(sb);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Invalid Input!");
                }
            string status;
            while (true)
            {
                Console.Write("Status: ");
                Console.ForegroundColor = ConsoleColor.Green;
                status = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (status.Equals("Active") || status.Equals("Disabled") || status.Equals("")) break;
                Console.WriteLine("Invalid! Please enter Active or Disabled");
            }
            var list = bLogic.searchCustomers(id, userId, name, balance, status);
            Console.WriteLine("==== SEARCH RESULTS ======");
            Console.WriteLine("Account ID User ID Holders Name    Type    Balance Status");
            for (int i = 0; i < list.us.Count; i++)
                for (int j = 0; j < list.ac.Count; j++)
                    if (list.us[i].Id == list.ac[j].UserId)
                        Console.WriteLine($"{String.Format("{0,-10:D}", list.ac[j].Id.ToString())} {String.Format("{0,-7:D}", list.us[i].Id.ToString())} {String.Format("{0,-15:D}", list.us[i].Name)} {String.Format("{0,-7:D}", list.ac[j].Type)} {String.Format("{0,-7:D}", list.ac[j].Balance.ToString())} {list.ac[j].Status}");
        }
        private void viewReport() //Function working on admin's View Report feature
        {
            Console.WriteLine("1---Accounts By Amount");
            Console.WriteLine("2---Accounts By Date");
            char option;
            while (true)
            {
                Console.Write("Please select one of the above options: ");
                Console.ForegroundColor = ConsoleColor.Green;
                option = Console.ReadKey().KeyChar;
                Console.ForegroundColor = ConsoleColor.White;
                if (option != '1' && option != '2')
                {
                    Console.WriteLine("\nInvalid Input!");
                    continue;
                }
                break;
            }
            switch (option)
            {
                case '1':
                    accByBalance();
                    break;
                case '2':
                    transByDate();
                    break;
                default:
                    break;
            }
        }
        private void accByBalance() //Function prints accounts having balance two amounts entered by admin
        {
            decimal minBalance;
            decimal maxBalance;
            while (true)
            {
                while (true)
                    try
                    {
                        Console.Write("\nEnter the minimum amount: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        minBalance = Convert.ToInt32(Console.ReadLine());
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Invalid Input!");
                    }
                while (true)
                    try
                    {
                        Console.Write("Enter the maximum amount: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        maxBalance = Convert.ToInt32(Console.ReadLine());
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Invalid Input!");
                    }
                if (minBalance > maxBalance)
                {
                    Console.WriteLine("Minimum Balance should less or equal to Maximum Balance");
                    continue;
                }
                break;
            }
            var list = bLogic.getAccByBalance(minBalance, maxBalance);
            Console.WriteLine("==== SEARCH RESULTS ======");
            Console.WriteLine("Account ID User ID Holders Name    Type    Balance Status");
            for (int i = 0; i < list.us.Count; i++)
                for (int j = 0; j < list.ac.Count; j++)
                    if (list.us[i].Id == list.ac[j].UserId)
                        Console.WriteLine($"{String.Format("{0,-10:D}", list.ac[j].Id.ToString())} {String.Format("{0,-7:D}", list.us[i].Id.ToString())} {String.Format("{0,-15:D}", list.us[i].Name)} {String.Format("{0,-7:D}", list.ac[j].Type)} {String.Format("{0,-7:D}", list.ac[j].Balance.ToString())} {list.ac[j].Status}");
        }
        private void transByDate() //Function prints transactions between two dates entered by admin
        {
            DateTime startDate;
            DateTime endDate;
            while (true)
            {
                while (true)
                    try
                    {
                        Console.Write("\nEnter the starting date: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        startDate = DateTime.Parse(Console.ReadLine());
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Invalid Input!");
                    }
                while (true)
                    try
                    {
                        Console.Write("Enter the ending date: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        endDate = DateTime.Parse(Console.ReadLine());
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Invalid Input!");
                    }
                if (startDate.Date > endDate.Date)
                {
                    Console.WriteLine("Start Date should less or equal to End Date");
                    continue;
                }
                break;
            }
            List<Transaction>  trans = bLogic.getTransByDate(startDate, endDate);
            Console.WriteLine("==== SEARCH RESULTS ======");
            Console.WriteLine("Transaction      User ID Holders Name    Amount  Date");
            for (int i = 0; i < trans.Count; i++)
                Console.WriteLine($"{String.Format("{0,-16:D}", trans[i].TransType)} {String.Format("{0,-7:D}", trans[i].UserId.ToString())} {String.Format("{0,-15:D}", trans[i].Name)} {String.Format("{0,-7:D}", trans[i].Amount.ToString())} {trans[i].Date.ToString("dd/MM/yyyy")}");
        }
    }
}
