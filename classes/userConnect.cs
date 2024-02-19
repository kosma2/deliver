using System.Data.SqlTypes;
using System.Text;
using Microsoft.Data.SqlClient;

namespace deliver
{
    partial class Program
    {
        public class userConnect : DbConnection
        {
            
            public override void DBdeleteOrder(int orderId)  // deletes the order and associated orderItems
            {   
                using(SqlConnection connection = GetConnection(SqlStr))
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try{
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM orders WHERE OrderId = @orderId";
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM orderItems WHERE OrderId = @orderId";
                command.ExecuteNonQuery();
                transaction.Commit();

                }catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
                
            }
            public void ShowOrdersNitems(int customerId)
            {
                List<List<String>> orders = DBListOrders(customerId);
                foreach (List<string> Item in orders)
                {
                    int orderId = Convert.ToInt32(Item[0]);
                    System.Console.WriteLine("Items for order id " + orderId);
                    List<string> items = DBListOrderItems(orderId);
                    foreach (string tems in items)
                    {
                        System.Console.WriteLine("Item Id is " + tems);
                    }
                }
            }
            public override (int, int) InterfaceCreateOrder()
            {
                //userConnect usConnect = new userConnect();
                List<object> dispItems = DBListItems();
                var subSetString = dispItems[1] as List<string>;
                foreach (string item in subSetString)
                {
                    System.Console.WriteLine(item);
                }
                System.Console.WriteLine("please order #");
                String userInput = System.Console.ReadLine();
                int inputItemId = 0;
                int.TryParse(userInput, out int number);
                var subSetInt = dispItems[0] as List<string>;
                if (number < dispItems.Count)
                {
                    inputItemId = subSetInt[0][number];
                }
                else
                {
                    System.Console.WriteLine("invalid number");
                }
                System.Console.WriteLine("how many you want?");
                int quant = Convert.ToInt32(Console.ReadLine());
                return (inputItemId, quant);

            }
            public override int DBcreateOrder(int itemId, int quantity) //returns orderId
            {
                int custId = 3;
                String custAddress = GetCustomerAddress(custId);
                System.Console.WriteLine(custAddress);
                Order order = new(custId, custAddress);
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    string query = "INSERT INTO orders (CustomerId, OrderDate, DeliveryAddress, DeliverStatus) VALUES (@custId, @orDate, @custAddy, @status);SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new(query, connection);
                    command.Parameters.AddWithValue("@custId", order.CustomerId);
                    command.Parameters.AddWithValue("orDate", order.OrderDate);
                    command.Parameters.AddWithValue("@custAddy", order.DeliveryAddress);
                    command.Parameters.AddWithValue("@status", order.DeliveryStatus);
                    connection.Open();
                    int resultOrderId;
                    using (command)
                    {
                        resultOrderId = Convert.ToInt32(command.ExecuteScalar());

                        //return resultOrderId;
                    }
                    DBcreateOrderItem(resultOrderId, itemId, quantity);
                    return 0;
                }


            }
            public override int DBcreateOrderItem(int orderId, int itemId, int quantity) //returns orderItemId
            {
                OrderItem orItem = new(orderId, itemId, quantity);
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    string query = "INSERT INTO orderItems (OrderId, ItemId, Quantity, Price) VALUES (@orderId, @itemId, @quant, @price); SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new(query, connection);
                    command.Parameters.AddWithValue("@itemId", orItem.ItemId);
                    command.Parameters.AddWithValue("@orderId", orItem.OrderId);
                    command.Parameters.AddWithValue("@quant", orItem.Quantity);
                    decimal price = GetItemPrice(orItem.ItemId);
                    decimal totalPrice = price * orItem.ItemId;
                    command.Parameters.AddWithValue("@price", totalPrice);
                    connection.Open();
                    using (command)
                    {
                        int resultOrderItemId = Convert.ToInt32(command.ExecuteScalar());
                        return resultOrderItemId;
                    }
                }
            }
            public override decimal GetItemPrice(int itemId)
            {
                using (SqlConnection connection = GetConnection(SqlStr))
                {
                    String sql = "SELECT ItemPrice FROM inventory WHERE ItemId = @itemId";
                    using (SqlCommand command = new(sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemId", itemId);
                        connection.Open();
                        decimal resultItemPrice = Convert.ToDecimal(command.ExecuteScalar());
                        return resultItemPrice;
                    }
                }
            }
            public String GetCustomerAddress(int custId)
            {
                SqlConnection connection = GetConnection(SqlStr);
                //SqlConnection connection = SqlStr;
                using (connection)
                {
                    String sqlCommand = "SELECT HomeAddress FROM customer WHERE CustomerId = @custId";
                    using (SqlCommand command = new(sqlCommand, connection))
                    {
                        command.Parameters.AddWithValue("@custId", custId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                String resultAddress = reader.GetString(0);
                                return resultAddress;
                            }
                            else
                            {
                                System.Console.WriteLine("nothing to read here");
                                return null;
                            }
                        }
                    }
                }
            }
            public List<List<String>> DBListOrders(int custId)  //lists orders based on CustomerId
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    String sqlStr = "SELECT OrderId, DeliveryAddress,DeliverStatus FROM orders WHERE CustomerId = @customer";
                    SqlCommand command = new(sqlStr, connection);
                    command.Parameters.AddWithValue("@customer", custId);
                    using (command)
                    {
                        connection.Open();
                        SqlDataReader read = command.ExecuteReader();
                        List<List<string>> orderList = new();
                        int orderCount = 0;
                        while (read.Read())
                        {
                            List<string> orderDetail = new List<string>
                            {
                                read.GetInt32(0).ToString(),
                                read.GetString(1),
                                read.GetString(2)
                            };
                            orderList.Add(orderDetail);
                        }
                        return orderList;
                    }
                }
            }
            public List<String> DBListOrderItems(int orderId)  // lists an OrderId's Items
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    String sqlStr = "SELECT ItemId FROM orderItems WHERE OrderId = @orderId";
                    SqlCommand command = new(sqlStr, connection);
                    command.Parameters.AddWithValue("@orderId", orderId);
                    using (command)
                    {
                        connection.Open();
                        SqlDataReader read = command.ExecuteReader();
                        List<string> itemList = new();
                        while (read.Read())
                        {
                            itemList.Add(Convert.ToString(read[0]));
                        }
                        return itemList;
                    }
                }
            }
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
            public override int DBCheckLogin(Member mem)//returns member id if logged in, "-1" for password mismatch, "-2" for no such user
            {

                SqlConnection connect = GetConnection(SqlStr);
                using (connect)
                {
                    // GETTING PASSWORD HASH
                    String sql = "SELECT PasswordHash, Salt, MemberId FROM member WHERE Login = @login";
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
                                System.Console.WriteLine("MemberId is " + resultMemId);
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

            public override void DBCreateMember(Member memb)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    //SQL STRING BUILD
                    StringBuilder sb = new();
                    sb.Append("USE master; ");
                    sb.Append("INSERT INTO member (Login, PasswordHash, Salt, Role) VALUES ");
                    sb.Append("(@login, @passHash, @salt, @role);");
                    sb.Append("SELECT SCOPE_IDENTITY();");
                    String sql = sb.ToString();

                    // HASH AND SALT
                    Byte[] salt = SecurityHelper.CreateSalt();
                    String passHash = SecurityHelper.HashPassword(memb.pass, salt);
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@login", memb.login);
                        command.Parameters.AddWithValue("@passHash", passHash);
                        command.Parameters.AddWithValue("@salt", salt);
                        command.Parameters.AddWithValue("@role", 1);

                        connection.Open();
                        int resultMemId = Convert.ToInt32(command.ExecuteScalar());
                        Console.WriteLine($"member id is {resultMemId}.");
                    }
                }
            }
            public override bool DBCreateCustomer(Customer cust)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    //SQL STRING BUILD
                    StringBuilder sb = new();
                    sb.Append("USE master; ");
                    sb.Append("INSERT INTO customer (FirstName, LastName, HomeAddress, Coords, DateCreated) VALUES ");
                    sb.Append("(@fName, @lName, @hAddress, @coords, @date);");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@fName", cust.firstName);
                        command.Parameters.AddWithValue("@lName", cust.lastName);
                        command.Parameters.AddWithValue("@hAddress", cust.homeAddress);
                        command.Parameters.AddWithValue("@coords", cust.coords);
                        command.Parameters.AddWithValue("@date", cust.dateCreated);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                        return rowsAffected > 0;
                    }
                }
            }
            public override void DBAddItem(Item item)
            {
            }
            public override void DBDeleteItem(string id)
            {
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
            }

        }
    }
}
