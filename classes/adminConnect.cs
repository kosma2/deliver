using System.Text;
using Microsoft.Data.SqlClient;

namespace deliver
{
    partial class Program
    {
        public class adminConnect : DbConnection
        {
            public override void DBdeleteOrder(int orderId)
            {}
            public override (int,int) InterfaceCreateOrder()
            {
                return (0,0);
            }
            public override int DBcreateOrder(int itemId, int quantity)
            {
               return 0;
            
            }
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
           public override List<object> DBListItems() // dispays all items in inventory [itemId][itemName]
            {
                SqlConnection connecti = GetConnection(SqlStr);
                using (connecti)
                {
                    String sql = "SELECT ItemId, ItemName FROM inventory;"; //, ItemDescr, ItemPrice FROM inventory;";
                    using (SqlCommand command = new SqlCommand(sql, connecti))
                    {
                        connecti.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                int rowCount = 0;
                                var IdAndName = new List<object>  // list of lists
                                {
                                    new List<int>(),                            // list of ItemId
                                    new List<string>()                          // list of ItemName
                                };
                                while (reader.Read())
                                {
                                    int conv = Convert.ToInt32(reader["ItemId"]);
                                    ((List<int>)IdAndName[0]).Add(conv);  // adds 
                                    ((List<string>)IdAndName[1]).Add((String)reader["ItemName"]);
                                }
                                return IdAndName;
                            }
                            else
                            {
                                Console.WriteLine("No rows found.");
                                reader.Close();
                                return null;
                            }
                        }
                    }
                }
            }
            public override int DBCheckLogin(Member mem)
            {

                SqlConnection connect = GetConnection(SqlStr);
                using (connect)
                {
                    // GETTING PASSWORD HASH
                    String sql = "SELECT PasswordHash, Salt FROM member WHERE Login = @login";
                    using (SqlCommand command = new(sql, connect))
                    {
                        connect.Open();
                        command.Parameters.AddWithValue("@login", mem.login);
                        using SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            string resultHash = reader.GetString(0);
                            byte[] resultSalt = new byte[32];
                            int resultMemId = reader.GetInt32(2);
                            reader.GetBytes(reader.GetOrdinal("Salt"), 0, resultSalt, 0, 32);
                            string inputHash = SecurityHelper.HashPassword(mem.pass, resultSalt);
                            if (inputHash == resultHash)
                            {
                                System.Console.WriteLine("user logged in");
                                return resultMemId;
                            }
                            else
                            {
                                System.Console.WriteLine("login failed");
                                return -1;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No user found with username: {mem.login}");
                            return -2;
                        }
                    }
                }

            }
 public override bool DBCreateCustomer(Customer cust)
            {            
                return true;
            }
            public override void DBCreateMember(Member memb)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    //SQL STRING BUILD
                    StringBuilder sb = new();
                    sb.Append("USE master; ");
                    sb.Append("INSERT INTO member (Login, PasswordHash, Salt) VALUES ");
                    sb.Append("(@login, @passHash, @salt);");
                    String sql = sb.ToString();

                    // HASH AND SALT
                    Byte[] salt = SecurityHelper.CreateSalt();
                    String passHash = SecurityHelper.HashPassword(memb.pass, salt);
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@login", memb.login);
                        command.Parameters.AddWithValue("@passHash", passHash);
                        command.Parameters.AddWithValue("@salt", salt);

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
                    sb.Append("INSERT INTO inventory (ItemId, ItemName, ItemDescr, ItemPrice, ItemDiment, ItemWeight) VALUES ");
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
            public override decimal GetItemPrice(int itemId)
            {return 0;}
            public override int DBcreateOrderItem(int orderId, int itemId, int quantity)
            {
                return 0;
            }
        }
    }
}
