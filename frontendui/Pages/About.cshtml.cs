using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Services.AppAuthentication;

namespace frontendui.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = Get();
        }

        public string Get()
        {
            var httpclient = new System.Net.Http.HttpClient(new System.Net.Http.HttpClientHandler()
            {
                //Proxy = new System.Net.WebProxy()
                //{
                //    Address = new Uri($"http://webproxy-inet.ms.com:8080"),
                //    BypassProxyOnLocal = true,
                //    UseDefaultCredentials = true
                //}
            });

            // Instantiate a new KeyVaultClient object, with an access token to Key Vault
            //var azureServiceTokenProvider1 = new AzureServiceTokenProvider();
            //var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider1.KeyVaultTokenCallback));

            //// Optional: Request an access token to other Azure services
            //var azureServiceTokenProvider2 = new AzureServiceTokenProvider();

            //var accessTokenTask = azureServiceTokenProvider2.GetAccessTokenAsync("a538ca81-3d33-4536-8f80-f271945da83d");

            //accessTokenTask.Wait();
            //var accessToken = accessTokenTask.Result;
            ////accessToken = await azureServiceTokenProvider2.GetAccessTokenAsync("https://targetwebapi.azurewebsites.net").ConfigureAwait(false);

            //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = httpclient.GetAsync("https://targetwebapi.azurewebsites.net/api/values").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return content;
            }
            return response.ReasonPhrase + Environment.NewLine; 
        }
    }
}
