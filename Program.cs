using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.IO;
using System.Net.Http;

namespace deliver
{
    partial class Program
    {

        public static void Main()
        {
            /*SqlConnectionStringBuilder builder = new();
            builder.DataSource = "localhost";
            builder.InitialCatalog = "master";
            builder.UserID = "kosma";
            builder.Password = "setANewPasswordPls";
            builder.TrustServerCertificate = true;*/
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
            adminConnect adCon = new adminConnect();
            string connectionString = config.GetConnectionString("DefaultConnection");
            adCon.SqlStr = connectionString;

            //adCon.DBAddMember(Login());
            adCon.DBCheckLogin(InputMemberCreate());

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
        public static String HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static Byte[] CreateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        public static Member InputMemberCreate()
        {
            System.Console.WriteLine("creating new member");
            Console.WriteLine("login please");
            String login = Console.ReadLine();
            System.Console.WriteLine("password please");
            String pass = Console.ReadLine();
            Member mem = new(login, pass);
            return mem;
        }
        public static Member Login()
        {
            System.Console.WriteLine("loggin you in");
            System.Console.WriteLine("login name please");
            String login = Console.ReadLine();
            System.Console.WriteLine("password pls");
            String pass = Console.ReadLine();
            Member mem = new(login, pass);
            return mem;
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
