using System.Text;
using Microsoft.Data.SqlClient;

namespace deliver
{
    partial class Program
    {
        public static Member InputCreateOrder()
        {
            System.Console.WriteLine("creating new order");
            Console.WriteLine("login please");
            String login = Console.ReadLine();
            System.Console.WriteLine("password please");
            String pass = Console.ReadLine();
            Member mem = new(login, pass);
            return mem;
        }
        public static Member InputMemberCreate()
        {
            System.Console.WriteLine("creating new member");
            Console.WriteLine("login please");
            String login = Console.ReadLine();
            System.Console.WriteLine("password please");
            String pass = Console.ReadLine();
            Member mem = new(login, pass);
            return mem;
        }
        public static Member Login()
        {
            System.Console.WriteLine("loggin you in");
            System.Console.WriteLine("login name please");
            String login = Console.ReadLine();
            System.Console.WriteLine("password pls");
            String pass = Console.ReadLine();
            Member mem = new(login, pass);
            return mem;
        }
        public static void InputCreateItem()
        {
            System.Console.WriteLine("pls item name");
            string itemName = Console.ReadLine();
            System.Console.WriteLine("pls item description");
            string itemDesc = Console.ReadLine();
            System.Console.WriteLine("pls item price");
            string itemPrice = Console.ReadLine();
            //Item itm = new(itemName, itemDesc, Convert.ToDecimal(itemPrice));
            System.Console.WriteLine("item " + itemName + " created");
        }
        public static Customer InputCreateCustomer(int memberId)
        {
            System.Console.WriteLine("plas customer name?");
            string custName = Console.ReadLine();
            System.Console.WriteLine("pls customer last name?");
            string custLName = Console.ReadLine();
            System.Console.WriteLine(" customer addy pls");
            string custAddy = Console.ReadLine();
            Customer cust = new(memberId, custName, custLName, custAddy);
            System.Console.WriteLine("customer " + custLName + " created");
            return cust;
        }
    }
}