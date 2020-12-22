using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace SecureClient
{
    class Program
    {
        static void Main(string[] args)
        {

            RunAsync().GetAwaiter().GetResult();
        }

        public static async Task RunAsync()
        {
            AuthConfig auth = AuthConfig.ReadJsonFromFile("appsettings.json");
            IConfidentialClientApplication app;
            
            app = ConfidentialClientApplicationBuilder.Create(auth.ClientId).WithClientSecret(auth.ClientSecret)
                .WithAuthority(new Uri(auth.Authority)).Build();
            string[] ResourceId = new string[] { auth.ResourceId };
            AuthenticationResult result;
            try
            {
                result = await app.AcquireTokenForClient(ResourceId).ExecuteAsync();
                Console.WriteLine(result.AccessToken);
                if (!string.IsNullOrEmpty(result.AccessToken))
                {
                    var httpClient = new HttpClient();
                    var defaultRequestHeaders = httpClient.DefaultRequestHeaders;
                    if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    defaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.AccessToken);

                    HttpResponseMessage response= await httpClient.GetAsync(auth.BaseAddress);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(json);
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
            }
           
        } 
    }
}
