using Microsoft.Extensions.Configuration;


namespace deliver
{
    partial class Program
    {
        public static void Main()
        {
            System.Console.WriteLine("1.admin 2.user");
            string a = Console.ReadLine();
            switch(a)
            {
                case "1":
                IAdminLogin();
                break;
                case "2":
                IUserLogin();
                break;
            }
            /*var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            UserSession uSession = null;
            GeoConnect geoConnection = new();
                        geoConnection.SqlStr = config.GetConnectionString("DefaultConnection");
                        double distance = geoConnection.GetDistance(1004,3);
                        System.Console.WriteLine($"distance between them is {distance}m.");*/
        }
    }
}
