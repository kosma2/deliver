using System.Data;
using System.Text;
using Azure.Core.GeoJson;
using Microsoft.Data.SqlClient;

namespace deliver
{
    partial class Program
    {
        public class GeoConnect : DbConnection
        {
            public UserSession CurrentSession { get; private set; }
            public void DBCreateGeoObject(string GeomType, string MarkerName, List<string> PointList, int Buffer) // INSERTS a GeoSpatial geometry
            {
                String SQLString ="";
                StringBuilder pointStringBuild = new();
                //pointStringBuild.Append("-74.0060 40.7128, -77.0369 38.9072");
                switch (GeomType)
                {
                    // building Stringbuilder string in format: geometry::STLineFromText('LINESTRING(-74.0060 40.7128, -77.0369 38.9072)', 4326)
                    case "polygon":
                        pointStringBuild.Insert(0, "POLYGON((");
                        SQLString = "INSERT INTO airmarker (ShapeName, MarkerName,GeoData,Buffer) VALUES (@GeomType, @MarkName, geometry::STPolyFromText(@WKL, 4326), @Buffer);";    //for geometry geometry::STPolyFromText , for geography geometry::STPolygonFromText
                        break;
                    case "line":    // creates a LINESTRING geometry
                        SQLString = "INSERT INTO airmarker (ShapeName, MarkerName, GeoData,Buffer) VALUES (@GeomType, @MarkName, geometry::STLineFromText('LINESTRING('+ @WKL +')', 4326), @Buffer);";
                        break;
                    case "point":
                        SQLString = "INSERT INTO airmarker (ShapeName, MarkerName, GeoData, Buffer) VALUES (@GeomType, @MarkName, geometry::STPointFromText('POINT('+ @WKL + ')', 4326), @Buffer);";
                        break;
                }
                foreach (string pt in PointList)
                {
                    pointStringBuild.Append(pt + ", ");
                }
                pointStringBuild.Remove(pointStringBuild.Length - 2, 2); //removes the last coma and space
                if(GeomType == "polygon"){pointStringBuild.Append("))");}  //polygon needs an extra ")" after points

                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    String query = SQLString;
                    SqlCommand command = new(query, connection);
                    command.Parameters.Add("@GeomType", SqlDbType.VarChar).Value = GeomType;
                    command.Parameters.Add("@MarkName", SqlDbType.VarChar).Value = MarkerName;
                    command.Parameters.AddWithValue("@WKL", SqlDbType.VarChar).Value = pointStringBuild.ToString();
                    command.Parameters.AddWithValue("@Buffer", SqlDbType.VarChar).Value =  Buffer;

                    connection.Open();
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            public void DBDeleteAirMarker(int markerId)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    StringBuilder sb = new();
                    sb.Append("DELETE FROM airmarker WHERE ID = @markerId");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@markerId", markerId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        System.Console.WriteLine(rowsAffected);
                    }
                }
            }
            /* du[plicate]         public double GetDistance(int CustomerId1, int CustomerId2)
            {
                //float distance = 0;
                using (SqlConnection connection = GetConnection(SqlStr))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"
                        DECLARE @geoPoint1 GEOMETRY;
                        DECLARE @geoPoint2 GEOMETRY;
                        SELECT @geoPoint1 = GeoPoint FROM customer WHERE CustomerId = @CustId1;
                        SELECT @geoPoint2 = GeoPoint FROM customer WHERE CustomerId = @CustId2;
                        DECLARE @distance float;
                        SET @distance = @geoPoint1.STDistance(@geoPoint2);
                        SELECT 
                            @distance as DistanceInMeters,
                            @geoPoint1.STAsText() as Customer1GeoPoint,
                            @geoPoint2.STAsText() as Customer2GeoPoint;";

                        command.Parameters.AddWithValue("@CustId1", CustomerId1);
                        command.Parameters.AddWithValue("@CustId2", CustomerId2);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Assuming there's at least one row returned
                            {
                                double distance = reader.GetDouble(reader.GetOrdinal("DistanceInMeters"));
                                string customer1GeoPoint = reader.GetString(reader.GetOrdinal("Customer1GeoPoint"));
                                string customer2GeoPoint = reader.GetString(reader.GetOrdinal("Customer2GeoPoint"));

                                Console.WriteLine($"Distance in Meters: {distance}");
                                Console.WriteLine($"Customer 1 GeoPoint: {customer1GeoPoint}");
                                Console.WriteLine($"Customer 2 GeoPoint: {customer2GeoPoint}");
                                return distance; // The distance in meters
                            }
                            else
                            {
                                Console.WriteLine("No data found.");
                                return 0; // The distance in meters
                            }
                        }
                    }

                }
            }*/
            //all derived methods

            public List<(int, string)> ShowAirMarkers()
            {
                List<(int, string)> markerInfo = new();     // List to hold all results
                using (SqlConnection connection = GetConnection(SqlStr))
                {
                    String sql = "SELECT ID, ShapeName FROM airmarker";
                    using (SqlCommand command = new(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int shapeId = reader.GetInt32(reader.GetOrdinal("ID"));
                                string shapeName = reader.GetString(reader.GetOrdinal("ShapeName"));
                                markerInfo.Add((shapeId, shapeName));
                            }
                            return markerInfo;
                        }
                    }
                }
            }
            public double GetDistance(int CustomerId1, int CustomerId2)
            {
                //float distance = 0;
                using (SqlConnection connection = GetConnection(SqlStr))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"
                        DECLARE @geoPoint1 GEOMETRY;
                        DECLARE @geoPoint2 GEOMETRY;
                        SELECT @geoPoint1 = GeoPoint FROM customer WHERE CustomerId = @CustId1;
                        SELECT @geoPoint2 = GeoPoint FROM customer WHERE CustomerId = @CustId2;
                        DECLARE @distance float;
                        SET @distance = @geoPoint1.STDistance(@geoPoint2);
                        SELECT 
                            @distance as DistanceInMeters,
                            @geoPoint1.STAsText() as Customer1GeoPoint,
                            @geoPoint2.STAsText() as Customer2GeoPoint;";

                        command.Parameters.AddWithValue("@CustId1", CustomerId1);
                        command.Parameters.AddWithValue("@CustId2", CustomerId2);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Assuming there's at least one row returned
                            {
                                double distance = reader.GetDouble(reader.GetOrdinal("DistanceInMeters"));
                                string customer1GeoPoint = reader.GetString(reader.GetOrdinal("Customer1GeoPoint"));
                                string customer2GeoPoint = reader.GetString(reader.GetOrdinal("Customer2GeoPoint"));

                                Console.WriteLine($"Distance in Meters: {distance}");
                                Console.WriteLine($"Customer 1 GeoPoint: {customer1GeoPoint}");
                                Console.WriteLine($"Customer 2 GeoPoint: {customer2GeoPoint}");
                                return distance; // The distance in meters
                            }
                            else
                            {
                                Console.WriteLine("No data found.");
                                return 0; // The distance in meters
                            }
                        }
                    }

                }
            }
            //all derived methods

            public override List<(int, string)> ShowCustomers()
            {
                List<(int, string)> custInfo = new();
                using (SqlConnection connection = GetConnection(SqlStr))
                {
                    String sql = "SELECT CustomerId, LastName FROM customer";
                    using (SqlCommand command = new(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int custId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                                string custName = reader.GetString(reader.GetOrdinal("LastName"));
                                custInfo.Add((custId, custName));
                            }
                            return custInfo;
                        }
                    }
                }
            }
            public override string GetItemName(int itemId)
            {
                using (SqlConnection connection = GetConnection(SqlStr))
                {
                    String sql = "SELECT ItemName FROM inventory WHERE ItemId = @itemId";
                    using (SqlCommand command = new(sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemId", itemId);
                        connection.Open();
                        string resultItemName = Convert.ToString(command.ExecuteScalar());
                        return resultItemName;
                    }
                }
            }
            public override int DBGetCustomerId(int memId)// retrieves MemberId from customer table
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    String sqlCommand = "SELECT CustomerId FROM customer WHERE CustomerId = @memId";
                    using (SqlCommand command = new(sqlCommand, connection))
                    {
                        command.Parameters.AddWithValue("@memId", memId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int resultMemId = reader.GetInt32(0);
                                return resultMemId;
                            }
                            else
                            {
                                System.Console.WriteLine("nothing to read here");
                                return -1;
                            }
                        }
                    }
                }
            }
            public override void DBdeleteOrder(int orderId)
            { }
            public override (int, int) InterfaceCreateOrder()
            {
                return (0, 0);
            }
            public override int DBcreateOrder(int custId, int itemId, int quantity)
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
            public override List<(int ItemId, String ItemName)> DBListItems() // dispays all items in inventory [itemId][itemName]
            {
                List<(int, string)> idAndName = new();

                return idAndName;
            }
            public override int DBCheckLogin(Member mem)// creates a usersession, returns member id if logged in, "-1" for password mismatch, "-2" for no such user
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
                                int custId = this.DBGetCustomerId(resultMemId);  // retrieve customer id from DB
                                CurrentSession = new(resultMemId, custId);  // create in-class user session
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
            public override void DBDeleteCustomer(int CustomerId)
            {
                SqlConnection connection = GetConnection(SqlStr);
                using (connection)
                {
                    StringBuilder sb = new();
                    sb.Append("DELETE FROM Customer WHERE CustomerId = @custId");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@custId", CustomerId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        System.Console.WriteLine(rowsAffected);
                    }
                }
            }
            public override decimal GetItemPrice(int itemId)
            { return 0; }
            public override int DBcreateOrderItem(int orderId, int itemId, int quantity)
            {
                return 0;
            }

        }
    }
}