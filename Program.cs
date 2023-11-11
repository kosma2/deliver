using System.Text;
using Microsoft.Data.SqlClient;

namespace deliver
{
    class Item
    {
        public int itemId = 0;
        public String itemName = "";
        public String itemDesc = "";
        public decimal itemPrice = 0;
        String itemDimens = "";
        int itemWeight = 0;
        public Item(string name, string desc, decimal price)
        {
            itemName = name;
            itemDesc = desc;
            itemPrice = price;
        }
    }
    class Customer
    {
        public string firstName = "";
        public string lastName = "";
        public string homeAddress = "";
        public string coords = "";
        public DateOnly dateCreated;
        public Customer(string firName, string lasName, string homeAdd, DateOnly dateCrtd)
        {
            firstName = firName;
            lastName = lasName;
            homeAddress = homeAdd;
            dateCreated = dateCrtd;
        }

    }
    class Program
    {

        public static void Main()
        {

            try
            {
                SqlConnectionStringBuilder builder = new();
                builder.DataSource = "localhost";
                builder.InitialCatalog = "master";
                builder.UserID = "kosma";
                builder.Password = "setANewPasswordPls";
                builder.TrustServerCertificate = true;
                System.Console.WriteLine("Connecting to database...");
                using SqlConnection connection = new(builder.ConnectionString);
                String sql = WriteItemToDB();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    System.Console.WriteLine("exectuted");
                }
                
                //////
                /*String sql = "SELECT ItemName, ItemDescr, ItemPrice FROM inventory;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows){
                            while (reader.Read())
                            {
                               
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        
                        reader.Close();
                    }
                }}*/

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            /* System.Console.WriteLine("1: Create item");
             System.Console.WriteLine("2: Create customer");
             System.Console.WriteLine("3: show items");
             string whatDo = Console.ReadLine();
             switch (whatDo)
             {
                 case "1":
                     InputCreateItem();
                     break;
                 case "2":
                     InputCreateCustomer();
                     break;
                 case "3":
                     ReadItems();
                     break;

                 default:
                     break;
             }*/
        }

        public static void InputCreateItem()
        {
            System.Console.WriteLine("pls item name");
            string itemName = Console.ReadLine();
            System.Console.WriteLine("pls item description");
            string itemDesc = Console.ReadLine();
            System.Console.WriteLine("pls item price");
            string itemPrice = Console.ReadLine();
            Item itm = new(itemName, itemDesc, Convert.ToDecimal(itemPrice));
            //WriteItemToDB(itm);
            System.Console.WriteLine("item " + itemName + " created");
        }
        public static String WriteItemToDB()
        {
            StringBuilder sb = new();
            sb.Append("USE master; ");
            sb.Append("INSERT INTO inventory (ItemName,ItemDesc,ItemPrice) VALUES ");
            sb.Append("(widget,widget for widgeting,123.00);");
            return(sb.ToString());
        }
        public static void InputCreateCustomer()
        {
            System.Console.WriteLine("plas customer name?");
            string custName = Console.ReadLine();
            System.Console.WriteLine("pls customer last name?");
            string custLastN = Console.ReadLine();
            System.Console.WriteLine(" customer addy pls");
            string custAddy = Console.ReadLine();
            Customer cust = new(custName, custLastN, custAddy, DateOnly.FromDateTime(DateTime.Now));
            System.Console.WriteLine("customer " + custLastN + " created");
        }
        public static void ShowItems()
        {
            //sql = "SELECT ItemName, ItemDescr, ItemPrice FROM inventory;";

        }
    }
}
