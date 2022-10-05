using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DynamicsWebApiDemo.Support
{
    public class DataCleaner
    {

        private const string FileDirectory = "\\data\\";
        private static readonly string RootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        
        /// <summary>
        /// Delete a row by using the name property (i.e. only works on accounts for now)
        /// The responses are not currenty checked for success or failure here
        /// </summary>
        /// <param name="fileName">the name of the file in data folder without the .json extension</param>
        public void DeleteAccount(string fileName)
        {
            string path = RootDirectory + FileDirectory + fileName + ".json";
            JObject json = JObject.Parse(File.ReadAllText(path));
                       
            string entityName = "";
            JToken value;
            if (json.TryGetValue("@logicalName", out value))
            {
                entityName = (string)value;
            }

            string name = "";
            JToken nameValue;
            if (json.TryGetValue("name", out nameValue))
            {
                name = (string)nameValue;
            }

            var getResponse = GetRequest(name, entityName).Result;
            JObject body = JObject.Parse(getResponse.ToString());
            string accountId = body.SelectToken("value[0].accountid").ToString();
            var deleteResponse = DeleteRequest(accountId, entityName).Result;
        }

        private async Task<string> GetRequest(string name, string entityName)
        {
            var filter = "$select=name&$filter=contains(name, '" + name + "')";
            var webConfig = new WebApiConfiguration();
            var messageUri = webConfig.ServiceRoot + entityName + "?" + filter;
            var response = WebApiRequest.SendMessageAsync(webConfig, HttpMethod.Get, messageUri).Result;
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> DeleteRequest(String accountId, string entityName)
        {
           var webConfig = new WebApiConfiguration();
           string messageUri = webConfig.ServiceRoot + entityName + "(" + accountId + ")";
           var response = WebApiRequest.SendMessageAsync(webConfig, HttpMethod.Delete, messageUri).Result;
           return await response.Content.ReadAsStringAsync();
        }
    }

}
