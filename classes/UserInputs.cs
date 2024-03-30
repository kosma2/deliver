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
        public static Member InputCreateMember()
        {
            System.Console.WriteLine("creating new member");
            Console.WriteLine("login please");
            String login = Console.ReadLine();
            System.Console.WriteLine("password please");
            String pass = Console.ReadLine();
            Member mem = new(login, pass);
            return mem;
        }
        public static Member Login()       //asks input for login and password, returns a Member with those values
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
            System.Console.WriteLine(" customer LAT pls");
            string lattitude = Console.ReadLine();
            System.Console.WriteLine(" customer LON pls");
            string longitude = Console.ReadLine();
            string coords = $"geometry::STPointFromText('POINT({lattitude} {longitude})', 4326)";
            Customer cust = new(memberId, custName, custLName, custAddy, coords);
            System.Console.WriteLine("customer " + custLName + " created");
            return cust;
        }
    }
}