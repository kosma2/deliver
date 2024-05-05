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
            System.Console.WriteLine(" customer LON pls");
            string longitude = Console.ReadLine();
            System.Console.WriteLine(" customer LAT pls");
            string lattitude = Console.ReadLine();
            string coords = $"{longitude} {lattitude}";
            Customer cust = new(memberId, custName, custLName, custAddy, coords);
            System.Console.WriteLine("customer " + custLName + " created");
            return cust;
        }
        public static (string, string, List<string>, int) InputCreateAirMarker() //takes Geo object type: line or polygon, and their points: 2 for line, more for polygon. If polygon, first point is also added last to close the shape.
        {
            int bufferSize = 0;
            List<String> pointList = new();
            System.Console.WriteLine("What type of shape?");
            String shape = Console.ReadLine();
            System.Console.WriteLine("Name for the shape marker?");
            String shapeName = Console.ReadLine();
            String firstPoint = null;
            while(true)
            {
                System.Console.WriteLine("Keep entering Points or [Enter]");
                String input = Console.ReadLine();
            if(string.IsNullOrEmpty(input))
            {
                if(shape == "polygon")
                {
                    pointList.Add(firstPoint);  // closes the polygon
                }

                break;
            }else{
                pointList.Add(input);
                if(firstPoint == null)
                {
                    firstPoint=input;   // keep the first point
                }
                
            }
            }
            return (shape,shapeName, pointList, bufferSize);
        }
        public static int InputDeleteAirmarker()
        {
            System.Console.WriteLine("Which air marker do you want to delete? (ID)");
            int inputId = Convert.ToInt32(Console.ReadLine());
            return inputId;
        }
    }
}