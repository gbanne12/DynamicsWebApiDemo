using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using DynamicsWebApiDemo.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace DynamicsWebApiDemo.StepDefinitions
{
    [Binding]
    public sealed class DataStepDefinitions
    {
        private const string FileDirectory = "\\data\\";
        private static readonly string RootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Given("I have created (.*)")]
        public async void GivenTheFirstNumberIs(string fileName)
        {

            /*  string path = RootDirectory +  FileDirectory + fileName + ".json";
              JObject json = JObject.Parse(File.ReadAllText(path));

              string entityName = "";
              JToken value;
              if (json.TryGetValue("@logicalName", out value))
              {
                  entityName = (string)value;
              }

              string messageBody = JsonConvert.SerializeObject(json);
              await WebApiRequest.SendMessageAsync(HttpMethod.Post, entityName, messageBody);*/

            string path = RootDirectory + FileDirectory + fileName + ".json";
            JObject json = JObject.Parse(File.ReadAllText(path));
            // JObject json = JObject.Parse(File.ReadAllText("./Data/John Doe.json"));

            string entityName = "";
            JToken value;
            if (json.TryGetValue("@logicalName", out value))
            {
                entityName = (string)value;
            }
            //var entityName = "accounts";

      
            
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