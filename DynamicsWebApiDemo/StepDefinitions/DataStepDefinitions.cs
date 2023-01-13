using System.Reflection;
using DynamicsWebApiDemo.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace DynamicsWebApiDemo.StepDefinitions
{
    [Binding]
    public sealed class DataStepDefinitions
    {
        private const string FileDirectory = "\\data\\";
        private static readonly string RootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Given("I have created (.*)")]
        public static void GivenIHaveCreated(string fileName)
        {
            var filePath = RootDirectory + FileDirectory + fileName + ".json";
            var json = JObject.Parse(File.ReadAllText(filePath));

            // Get the entity to update from the json data file
            var entityName = "";
            if (json.TryGetValue("@logicalName", out JToken value))
            {
                entityName = (string)value;
            }

            var requestBody = JsonConvert.SerializeObject(json);
            var config = new WebApiConfiguration();
            var messageUri = config.ServiceRoot + entityName;
            var response = WebApiRequest.SendMessageAsync(config, HttpMethod.Post, messageUri, requestBody).Result;
            
            // Format and then output the JSON response to the console.
            bool hasResponseBody = response.IsSuccessStatusCode && 
                                      !response.StatusCode.ToString().Equals("NoContent");
            if (hasResponseBody)
            {
                var responseBody = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                System.Diagnostics.Debug.WriteLine(responseBody.ToString());
            }

            if (response.Headers.Contains("OData-EntityId"))
            {
                System.Diagnostics.Debug.WriteLine(response.ToString());
            }
        }
    }
}