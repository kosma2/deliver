using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
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
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = "localhost";
            builder.InitialCatalog = "master";
            builder.UserID = "kosma";
            builder.Password = "setANewPasswordPls";
            builder.TrustServerCertificate = true;

            adminConnect adCon = new adminConnect();
            //adCon.Connect(builder.ConnectionString);
            
            String itId = "2";
            String itName = "regulatorioum";
            String itDes = "regulatorium for regulating";
            String itPrice = "34.65";
            String itDiment = "4x4x6";
            String itWeight = "45";
            //adCon.DBAddItem(builder.ConnectionString, itId, itName, itDes, itPrice, itDiment, itWeight);
            adCon.DBdispItems(builder.ConnectionString);
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
        public abstract class DbConnection
        {
            String connString;
            //public abstract void Connect(String connString);
            public abstract void DBdispItems(String SqlStr);
            public abstract void DBAddItem(String SqlStr, String id, String nam, String Des, String pric, String Dims, String weit);
            public abstract void DBDeleteItem(String connection, String id);
        }
        public static SqlConnection getConnection(String SqlStr)
        {
            try
            {
                System.Console.WriteLine("Connecting to database...");
                SqlConnection connection = new(SqlStr);
                return connection;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                System.Console.WriteLine("connection returned nyull");
                return null;
            }
        }
        public class adminConnect : DbConnection
        {
            /*public override void Connect(String connString)
            {
                try
                {
                    System.Console.WriteLine("Connecting to database...");
                    using SqlConnection connection = new(connString);
                    connection.Open();
                    System.Console.WriteLine("connection open");
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    System.Console.WriteLine("connection returned nyull");
                    //return null;
                }
            }*/
            public override void DBdispItems(String SqlStr)
            {
                SqlConnection connecti = getConnection(SqlStr);
                using (connecti)
                {
                    String sql = "SELECT ItemName, ItemDescr, ItemPrice FROM inventory;";
                    using (SqlCommand command = new SqlCommand(sql, connecti))
                    {
                        connecti.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        System.Console.WriteLine(reader[i]);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("No rows found.");
                                reader.Close();
                            }
                        }
                    }
                }
            }
            public override void DBAddItem(String SqlStr, String id, String nam, String des, String pric, String dims, String weit)
            {
                SqlConnection connection = getConnection(SqlStr);
                using(connection)
                {
                    StringBuilder sb = new();
                    sb.Append("USE master; ");
                    sb.Append("INSERT INTO inventory (TableNameId, ItemName, ItemDescr, ItemPrice, ItemDiment, ItemWeight) VALUES ");
                    sb.Append("(@id, @itName, @itDescr, @itPrice, @itDimen, @itWeight);");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {   
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@itName", nam);
                        command.Parameters.AddWithValue("@itDescr", des);
                        command.Parameters.AddWithValue("@itPrice", pric);
                        command.Parameters.AddWithValue("@itDimen", dims);
                        command.Parameters.AddWithValue("@itWeight", weit);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                    }
                }
            }
            public override void DBDeleteItem(string SqlStr, string id)
            {
                SqlConnection connection = getConnection(SqlStr);
                using(connection)
                {
                    StringBuilder sb = new();
                    sb.Append("DELETE FROM inventory WHERE id = @id");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {   
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                    }
                }
                
            }
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
            System.Console.WriteLine("item " + itemName + " created");
        }
        
        public static void DBUpdateCustomer(SqlConnection connection)
        {
            String userToUpdate = "juju";
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE Customers SET Location = N'Some Place St' WHERE LastName = @lastName");
            String sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@lastName", userToUpdate);
                int rowsAffected = command.ExecuteNonQuery();
                System.Console.WriteLine(rowsAffected);
            }
        }
        public static void DBDeleteCustomer(SqlConnection connection)
        {
            String userToDelete = "juju";
            StringBuilder sb = new();
            sb.Append("DELETE FROM Customers WERE LastName = @lastName");
            String sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@lastName", userToDelete);
                int rowsAffected = command.ExecuteNonQuery();
                System.Console.WriteLine(rowsAffected);
            }
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

    }
}
