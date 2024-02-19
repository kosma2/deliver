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
            usConnect.DBdeleteOrder(1);
            
            /*UserSession session = new UserSession(3); // static memberID for testing
            (int itemId,int quant) = usConnect.InterfaceCreateOrder();*/
           
        }


    }
}
