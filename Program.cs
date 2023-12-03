using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Data.SqlClient;

namespace deliver
{
    class Item
    {
        public String itemId { get; set; } = "";
        public String itemName { get; set; } = "";
        public String itemDesc { get; set; } = "";
        public String itemPrice { get; set; } = "";
        public String itemDimens { get; set; } = "";
        public String itemWeight { get; set; } = "";
        public Item(String id, String nam, String des, String pric, String dims, String weit)
        {
            itemId = id;
            itemName = nam;
            itemDesc = des;
            itemPrice = pric;
            itemDimens = dims;
            itemWeight = weit;
        }
    }
    class Customer
    {
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string homeAddress { get; set; } = "";
        public string coords { get; set; } = "";
        public DateOnly dateCreated { get; set; }

        public Customer(string firName, string lasName, string homeAdd, DateOnly dateCrtd)
        {
            firstName = firName;
            lastName = lasName;
            homeAddress = homeAdd;
            dateCreated = dateCrtd;
        }
    }
    class Member
    {
        public string login { get; set; } = "";
        public string pass { get; set; } = "";
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
            adCon.SqlStr = builder.ConnectionString;
            
            InputUserCreate();

            Member memb = createMember(login);
            //adCon.DBAddMember(memb);

            //adCon.DBdispItems(builder.ConnectionString);
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
        public static void InputUserCreate()
        {
            Console.WriteLine("login please");
            String login = Console.ReadLine();
            System.Console.WriteLine("password please");
            String pass = Console.ReadLine();
            (String, String)  tup = (login, pass);
            createMember(tup);

        }
        public static Member createMember(String login, String pass)
        {
            Member member = new();
            member.login = login;
            member.pass = pass;
            return member;
        }
        public abstract class DbConnection
        {

            public String SqlStr;
            //public abstract void Connect(String connString);
            public abstract void DBdispItems();
            public abstract void DBAddItem(Item item);
            public abstract void DBDeleteItem(String id);
            public abstract void DBUpdateCustomer(String id);
            public abstract void DBDeleteCustomer(String id);
            public abstract void DBAddMember(Member mem);
        }
        public static SqlConnection GetConnection(String SqlStr)
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
            public override void DBdispItems()
            {
                SqlConnection connecti = GetConnection(SqlStr);
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
            public override void DBAddMember(Member memb)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    StringBuilder sb = new();
                    sb.Append("USE master; ");
                    sb.Append("INSERT INTO member (Login, Password) VALUES ");
                    sb.Append("(@login, @pass);");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@login", memb.login);
                        command.Parameters.AddWithValue("@pass", memb.pass);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                    }
                }
            }
            public override void DBAddItem(Item item)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    StringBuilder sb = new();
                    sb.Append("USE master; ");
                    sb.Append("INSERT INTO inventory (TableNameId, ItemName, ItemDescr, ItemPrice, ItemDiment, ItemWeight) VALUES ");
                    sb.Append("(@id, @itName, @itDescr, @itPrice, @itDimen, @itWeight);");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", item.itemId);
                        command.Parameters.AddWithValue("@itName", item.itemName);
                        command.Parameters.AddWithValue("@itDescr", item.itemDesc);
                        command.Parameters.AddWithValue("@itPrice", item.itemPrice);
                        command.Parameters.AddWithValue("@itDimen", item.itemDimens);
                        command.Parameters.AddWithValue("@itWeight", item.itemWeight);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                    }
                }
            }
            public override void DBDeleteItem(string id)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
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
            public override void DBUpdateCustomer(String id)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
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
            }
            public override void DBDeleteCustomer(String id)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
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
            //Item itm = new(itemName, itemDesc, Convert.ToDecimal(itemPrice));
            System.Console.WriteLine("item " + itemName + " created");
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
