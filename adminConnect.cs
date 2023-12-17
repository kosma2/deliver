using System.Text;
using Microsoft.Data.SqlClient;

namespace deliver
{
    partial class Program
    {
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
            public override void DBCheckLogin(Member mem)
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
                            reader.GetBytes(reader.GetOrdinal("Salt"), 0, resultSalt, 0, 32);
                            string inputHash = HashPassword(mem.pass, resultSalt);
                    if(inputHash == resultHash)
                    {
                        System.Console.WriteLine("user logged in");
                    }
                    else
                    {
                        System.Console.WriteLine("login failed");
                    }
                        }
                        else
                        {
                            Console.WriteLine($"No user found with username: {mem.login}");
                        }
                    }
                }
                
            }
            
            public override void DBAddMember(Member memb)
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
                    Byte[] salt = CreateSalt();
                    String passHash = HashPassword(memb.pass, salt);
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

    }
}
