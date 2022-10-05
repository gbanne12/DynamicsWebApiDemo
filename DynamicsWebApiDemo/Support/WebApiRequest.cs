using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;


namespace DynamicsWebApiDemo.Support
{
    internal class WebApiRequest
    {
        /// <summary>
        /// Make a http request to Dataverse defined in the appsettings.json
        /// </summary>
        /// <param name="webConfig">values from appsettings.json</param>
        /// <param name="httpMethod">the request type</param>
        /// <param name="messageUri">the url or the request including filter and select info</param>
        /// <param name="messageBody">the body required for a POST request</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendMessageAsync(
            WebApiConfiguration webConfig, HttpMethod httpMethod, string messageUri, string messageBody = null)
        {
            var accessToken = await GetAccessToken(webConfig);

            // Create an HTTP message with the required WebAPI headers populated.
            var client = new HttpClient();
            var message = new HttpRequestMessage(httpMethod, messageUri);
            message.Headers.Add("OData-MaxVersion", "4.0");
            message.Headers.Add("OData-Version", "4.0");
            message.Headers.Add("Prefer", "odata.include-annotations=*");
            message.Headers.Add("Accept", "application/json");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (messageBody != null)
            {
                message.Content = new StringContent(messageBody, Encoding.UTF8, "application/json");
            }

            var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode && !response.StatusCode.ToString().Equals("NoContent"))
            {
                var result = response.Content.ReadAsStringAsync().Result;
                JObject body = JObject.Parse(result);
                System.Diagnostics.Debug.WriteLine(body.ToString());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("The request failed with a status of '{0}'", response.ReasonPhrase);
            }

            return response;
        }


        public static async Task<string> GetAccessToken(WebApiConfiguration webConfig)
        {
            var credentials = new ClientCredential(webConfig.ClientId, webConfig.Secret);
            var authContext = new AuthenticationContext("https://login.microsoftonline.com/" + webConfig.TenantId);
            var result = await authContext.AcquireTokenAsync(webConfig.ResourceUri, credentials);
            return result.AccessToken;
        }
    }
}
