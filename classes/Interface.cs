using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using Azure.Core.GeoJson;
using Microsoft.Data.SqlClient;
namespace deliver
{
    partial class Program
    {
        public static void IUserLogin()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            UserSession uSession = null;
            userConnect usConnect = new userConnect();
            usConnect.SqlStr = config.GetConnectionString("DefaultConnection");

            bool exit = false;
            while (!exit)
            {
                System.Console.WriteLine("choose wisely");
                System.Console.WriteLine("1. Create Member login");
                System.Console.WriteLine("2. Make me Customer");
                System.Console.WriteLine("3. Login");
                System.Console.WriteLine("4. display products");
                System.Console.WriteLine("5. order product");
                System.Console.WriteLine("6. list orders and items");
                System.Console.WriteLine("7. Log out");

                System.Console.WriteLine("8. Exit");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": // asks for login info and creates a member login
                        if (uSession == null)
                        {
                            usConnect.DBCreateMember(InputCreateMember());
                        }
                        break;
                    case "2":  // makes logged in member into a customer
                        if (uSession != null)
                        {
                            usConnect.DBCreateCustomer(InputCreateCustomer(uSession.MemberId));
                        }
                        else
                        {
                            System.Console.WriteLine("you are not logged in");
                        }
                        break;
                    case "3":
                        if (uSession == null)
                        {
                            ///this is done in check login now
                            int memberId = usConnect.DBCheckLogin(Login());
                            if (memberId == -2)
                            {
                                System.Console.WriteLine("no such user");
                            }
                            else
                            {
                                int customerId = usConnect.DBGetCustomerId(memberId);
                                if (memberId > 0)
                                {
                                    uSession = new(memberId, customerId);
                                }
                                System.Console.WriteLine($"member # {uSession.MemberId}, customer # {uSession.CustomerId}");
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("you are already logged in");
                        }
                        break;
                    case "4":
                        if (uSession != null)
                        {
                            var items = usConnect.DBListItems();
                            foreach (var item in items)  // is a list of [itemId][itemName]
                            {
                                System.Console.WriteLine("item id is " + item.ItemId + "item name is " + (string)item.ItemName);
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("log in first");
                        }
                        break;
                    case "5":
                        if (uSession != null)//~~~check if customerid > 0
                        {
                            (int itemId, int quant) = usConnect.InterfaceCreateOrder();
                            usConnect.DBcreateOrder(uSession.CustomerId, itemId, quant);
                        }
                        else
                        {
                            System.Console.WriteLine("log in first");
                        }
                        break;
                    case "6": //lists customer's orders and their items
                        List<List<string>> orders = usConnect.DBListOrders(usConnect.CurrentSession.CustomerId); //list of customer's orders
                        foreach (List<String> item in orders)           //for each order
                        {
                            List<string> orderItems = usConnect.DBListOrderItems(Convert.ToInt32(item[0]));  //List of item IDs of an order
                            foreach (string itmId in orderItems)        //for each item
                            {
                                string itemName = usConnect.GetItemName(Convert.ToInt32(itmId));            // get the item's name
                                System.Console.WriteLine(itemName);
                            }
                        }
                        break;
                    case "7":
                        uSession = null;
                        break;
                    case "8":
                        exit = true;
                        break;
                }
            }
        }
        public static void IAdminLogin()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            UserSession uSession = null;
            adminConnect adConnect = new adminConnect();
            adConnect.SqlStr = config.GetConnectionString("DefaultConnection");

            bool exit = false;
            while (!exit)
            {
                System.Console.WriteLine("choose wisely");
                System.Console.WriteLine("1. Log in");
                System.Console.WriteLine("2. Delete customer");
                System.Console.WriteLine("3. Display customers");
                System.Console.WriteLine("4. Find distance");
                System.Console.WriteLine("5. Make air geometry");
                System.Console.WriteLine("6. List air geometry");
                System.Console.WriteLine("7. Delete air geometry");
                System.Console.WriteLine("8. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {

                    case "1":
                        if (uSession == null)
                        {
                            ///this is done in check login now
                            int memberId = adConnect.DBCheckLogin(Login());
                            int customerId = adConnect.DBGetCustomerId(memberId);
                            if (memberId > 0)
                            {
                                uSession = new(memberId, customerId);
                            }
                            System.Console.WriteLine("session # " + uSession.MemberId);
                        }
                        else
                        {
                            System.Console.WriteLine("you are already logged in");
                        }
                        break;
                    case "2": //Deletes customer by custId
                        if (uSession != null)
                        {
                            System.Console.WriteLine("Customer ID please");
                            string id = Console.ReadLine();
                            adConnect.DBDeleteCustomer(Convert.ToInt32(id));
                        }
                        break;
                    case "3":  //disiplays all customers
                        List<(int, string)> customers = adConnect.ShowCustomers();
                        foreach ((int, string) item in customers)
                            System.Console.WriteLine($"customer id: {item.Item1} Name: {item.Item2}");
                        break;
                    case "4": //finds distance between 2 customers
                        //display customers
                        List<(int, string)> customersD = adConnect.ShowCustomers();
                        foreach ((int, string) item in customersD)
                            System.Console.WriteLine($"customer id: {item.Item1} Name: {item.Item2}");
                        //choose 2
                        System.Console.WriteLine("Id for customer 1");
                        string cus1 = Console.ReadLine();
                        System.Console.WriteLine("Id for customer 2");
                        string cus2 = Console.ReadLine();
                        int c1 = Convert.ToInt32(cus1);
                        int c2 = Convert.ToInt32(cus2);
                        GeoConnect geoConnection = new();
                        geoConnection.SqlStr = config.GetConnectionString("DefaultConnection");
                        double distance = geoConnection.GetDistance(c1, c2);
                        System.Console.WriteLine($"distance between them is {distance}m.");
                        break;
                    case "5":
                        GeoConnect geoConnection1 = new();
                        geoConnection1.SqlStr = config.GetConnectionString("DefaultConnection");

                        //List<string> points = [coords1, coords2];
                        int buffer = 100;
                        var (geomType, shapeName, stringList, buff) = InputCreateAirMarker();
                        geoConnection1.DBCreateGeoObject(geomType, shapeName, stringList, buffer);
                        break;
                    case "6":
                        GeoConnect geoConnection2 = new();
                        geoConnection2.SqlStr = config.GetConnectionString("DefaultConnection");
                        List<(int, string, string,string)> markers = geoConnection2.ShowAirMarkers();
                        foreach ((int, string, string, string) item in markers)
                            System.Console.WriteLine($"marker id: {item.Item1} Shape Name: {item.Item2} Marker Name: {item.Item3} Geo: {item.Item4}");
                        break;
                    case "7":
                        GeoConnect geoConnection3 = new();
                        geoConnection3.SqlStr = config.GetConnectionString("DefaultConnection");
                        int inputId = InputDeleteAirmarker();
                        geoConnection3.DBDeleteAirMarker(inputId);
                        break;
                    
                    case "8":
                        exit = true;
                        break;
                }
            }
        }
    }
}