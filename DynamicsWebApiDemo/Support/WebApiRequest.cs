using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;


namespace DynamicsWebApiDemo.Support
{
    internal class WebApiRequest
    {

        public static async Task<HttpResponseMessage> SendMessageAsync(HttpMethod httpMethod, string entityName, string messageBody = null)
        {
                    // Obtain the app registration and service configuration values from the App.config file.
            var webConfig = new WebApiConfiguration();
            //var accessToken = await GetAccessToken(webConfig);
            var accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSIsImtpZCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSJ9.eyJhdWQiOiJodHRwczovL29yZ2MwYzJhODUxLmFwaS5jcm00LmR5bmFtaWNzLmNvbSIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0LzUwOGIzYjZhLTc1NDEtNDJhNy1hMTUzLTljMjQ1NDk3NjBiZC8iLCJpYXQiOjE2NjQ4OTM5MTksIm5iZiI6MTY2NDg5MzkxOSwiZXhwIjoxNjY0ODk4NzkzLCJhY3IiOiIxIiwiYWlvIjoiQVRRQXkvOFRBQUFBZklYQWxpMXcvUEh2V2VjTm1ZTVRJUWdLeTMvU3YzZ2hpcXB6VnNKVVQ1Q01SSm9IMXRNbkx5QTF0TkYxMkUvVSIsImFtciI6WyJwd2QiXSwiYXBwaWQiOiI1MWY4MTQ4OS0xMmVlLTRhOWUtYWFhZS1hMjU5MWY0NTk4N2QiLCJhcHBpZGFjciI6IjAiLCJmYW1pbHlfbmFtZSI6IkJhbm5lcm1hbiIsImdpdmVuX25hbWUiOiJHYXJ5IiwiaXBhZGRyIjoiODYuMTY1LjQxLjE4OCIsIm5hbWUiOiJHYXJ5IEJhbm5lcm1hbiIsIm9pZCI6IjM1MjQ1NzY0LTE3OWItNGYzYS05Njk4LWNiZTY4OWFiM2RmNSIsInB1aWQiOiIxMDAzMjAwMjI4NThBRjc2IiwicmgiOiIwLkFZSUFhanVMVUVGMXAwS2hVNXdrVkpkZ3ZRY0FBQUFBQUFBQXdBQUFBQUFBQUFDVkFKSS4iLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiJNeUU3ZGczSWtYbjdmOW13UWdXMFpCMTlBYkNqcDZRZHY1LTVkclQ4Y2w4IiwidGlkIjoiNTA4YjNiNmEtNzU0MS00MmE3LWExNTMtOWMyNDU0OTc2MGJkIiwidW5pcXVlX25hbWUiOiJiYW5uZXJnYUB3bjd5ci5vbm1pY3Jvc29mdC5jb20iLCJ1cG4iOiJiYW5uZXJnYUB3bjd5ci5vbm1pY3Jvc29mdC5jb20iLCJ1dGkiOiJBeWdaZEFNVUNrT21fMkhnc2ZXb0FBIiwidmVyIjoiMS4wIn0.LJSO7tBLFtx-L1lrvk2MB05dFMLZCy8LJNmWYVdlUNrTmXPGUN0lVWdtWQXxp1GRnGdPZEbiW7fT6silcdh5fvG4osoiganHZefHJh2kQ4Phokcb_AJr3y2pOzWotKd93hQkzFZERGrWcR8n8RDpz6AwkJ7W8A2dlVlMN8m_vwBqJTumfZnwjcaBRo6R_tfCFh4EzzC5yT9xUW4WA4S6d8lIQyyVZXM27ERlLvVEv8nPKL56a26Sm2DFcvbpKz5bJ2NuA0EVKtNKuiB9Yv1swgyCB75MNO1pqaOPyP8bJkMTZOPmk5cV969_LSLfE_cJh8DWBT0AVA_sqV-9vorskw";

            var messageUri = webConfig.ServiceRoot + entityName;

            // Create an HTTP message with the required WebAPI headers populated.
            var client = new HttpClient();
            var message = new HttpRequestMessage(httpMethod, messageUri);
            message.Headers.Add("OData-MaxVersion", "4.0");
            message.Headers.Add("OData-Version", "4.0");
            message.Headers.Add("Prefer", "odata.include-annotations=*");
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

            try
            {
                var result0 = await authContext.AcquireTokenAsync(webConfig.ResourceUri, credentials);
                var result2 = await authContext.AcquireTokenAsync(webConfig.ResourceUri, credentials);
            } 
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); 
            }

            var result = await authContext.AcquireTokenAsync(webConfig.ResourceUri, credentials);
            return result.AccessToken;
        }
    }
}
