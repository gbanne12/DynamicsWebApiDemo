using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace DynamicsWebApiDemo.Support
{
    internal class WebApiConfiguration
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string TenantId { get; set; }
        public string ResourceUri { get; set; }
        public string ServiceRoot { get; set; }

        public WebApiConfiguration()
        {
            var builder = new ConfigurationBuilder();
            var config = JsonConfigurationExtensions.AddJsonFile(builder, "appsettings.json").Build();


            Console.WriteLine($"clientID = {config.GetConnectionString("Client-ID")}");

            // var appSettings = ConfigurationManager.AppSettings;
            /*ClientId = appSettings["Client-ID"];
            Secret = appSettings["Client-Secret"];
            TenantId = appSettings["Tenant-ID"];
            ResourceUri = appSettings["Resource-URL"];
            ServiceRoot = appSettings["Service-Root"];*/

            ClientId = config.GetConnectionString("Client-ID");
            Secret = config.GetConnectionString("Client-Secret");
            TenantId = config.GetConnectionString("Tenant-ID");
            ResourceUri = config.GetConnectionString("Resource-URL");
            ServiceRoot = config.GetConnectionString("Service-Root");
        }
    }
}
