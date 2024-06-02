using Microsoft.Extensions.Configuration;


namespace deliver
{
    partial class Program
    {
        public static void Main()
        {
            System.Console.WriteLine("1.admin 2.user 3.nav");
            string a = Console.ReadLine();
            switch (a)
            {
                case "1":
                    IAdminLogin();
                    break;
                case "2":
                    IUserLogin();
                    break;
                case "3":
                    Nav();
                    break;
            }

        }
        public static void Nav()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            UserSession uSession = null;
            GeoConnect geoConnection = new();
            geoConnection.SqlStr = config.GetConnectionString("DefaultConnection");
            //geoConnection.GetNodeDistance("n1", "n2");
            List<Node> nodeList = geoConnection.DBGetGraphData();
            
            var dijkstra = new Dijkstra();
            Node startingNode = nodeList.FirstOrDefault(node => node.Id == "n1");
            dijkstra.Execute(startingNode, nodeList);
        }
    }
}
