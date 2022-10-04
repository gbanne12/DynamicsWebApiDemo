using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicsWebApiDemo.Support
{ 

    internal class ExampleRun
    {
        static void Main(string[] args)
        {
            var entityName = "accounts";
            JObject json = JObject.Parse(File.ReadAllText("./Data/account.json"));
            string messageBody = JsonConvert.SerializeObject(json);

            var response = WebApiRequest.SendMessageAsync(HttpMethod.Post, entityName, messageBody).Result;

            // Format and then output the JSON response to the console.
            if (response.IsSuccessStatusCode)
            {
                JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine(body.ToString());
            }
            else
            {
                Console.WriteLine("The request failed with a status of '{0}'", response.ReasonPhrase);
            }
        }

    }
}
