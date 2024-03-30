using Microsoft.Extensions.Configuration;

namespace deliver
{
    partial class Program
    {
        public static bool loggedIn = false;

        public static void Main()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            userConnect usConnect = new userConnect();
            usConnect.SqlStr = config.GetConnectionString("DefaultConnection");
            UserSession uSession = null; 
            

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
                System.Console.WriteLine("7. Exit");
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
                        }else
                        {
                            System.Console.WriteLine("you are no logged in");
                        }
                        break;
                    case "3":
                        if (uSession == null)
                        {
                            ///this is done in check login now
                            int memberId = usConnect.DBCheckLogin(Login());
                            int customerId = usConnect.DBGetCustomerId(memberId);
                            uSession = new(memberId, customerId);
                            loggedIn = uSession.MemberId > 0;
                            System.Console.WriteLine("session # " + uSession.MemberId);
                        }else{
                            System.Console.WriteLine("you are already logged in");
                        }
                        break;
                    case "4":
                        if (loggedIn)
                        {
                            var items = usConnect.DBListItems();
                            foreach(var item in items)  // is a list of [itemId][itemName]
                            {
                                System.Console.WriteLine("item id is "+ item.ItemId + "item name is " + (string)item.ItemName);
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("log in first");
                        }
                        break;
                    case "5":
                        if (loggedIn)//~~~check if customerid > 0
                        {
                            (int itemId, int quant) = usConnect.InterfaceCreateOrder();
                            usConnect.DBcreateOrder(itemId, quant);
                        }
                        else
                        {
                            System.Console.WriteLine("log in first");
                        }
                        break;
                    case "6": //lists customer's orders and their items
                        /*List<List<string>> orders = usConnect.DBListOrders(usConnect.CurrentSession.CustomerId);
                        foreach (List<String> item in orders)
                        {
                            List<string> orderItems = usConnect.DBListOrderItems(Convert.ToInt32(item[0]));  //List of item IDs of an order
                            foreach (string itmId in orderItems)
                            {
                                string itemName = usConnect.GetItemName(Convert.ToInt32(itmId));
                                System.Console.WriteLine(itemName);
                            }
                        }
                        break;*/
                    case "7":
                        exit = true;
                        break;
                }
            }
        }


    }
}
