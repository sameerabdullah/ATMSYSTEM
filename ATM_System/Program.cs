using System.Collections.Generic;
using System;
using Views;
using System.IO;
using System.Text;
using DAL;
using Models;

namespace ATM_System
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Clear();
            (new View()).MainView();
        }
    }
}
