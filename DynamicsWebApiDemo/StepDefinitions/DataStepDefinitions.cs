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
        public void GivenTheFirstNumberIs(string fileName)
        {
            string path = RootDirectory + FileDirectory + fileName + ".json";
            JObject json = JObject.Parse(File.ReadAllText(path));

            // Get the entity to update from the json data file
            string entityName = "";
            JToken value;
            if (json.TryGetValue("@logicalName", out value))
            {
                entityName = (string)value;
            }
               
            string messageBody = JsonConvert.SerializeObject(json);
            var webConfig = new WebApiConfiguration();
            string messageUri = webConfig.ServiceRoot + entityName;
            var response = WebApiRequest.SendMessageAsync(webConfig, HttpMethod.Post, messageUri, messageBody).Result;

            // Format and then output the JSON response to the console.
            if (response.IsSuccessStatusCode  && !response.StatusCode.ToString().Equals("204"))
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