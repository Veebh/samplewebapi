using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;

namespace samplewebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private string accessToken;

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> GetAsync()
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
            var azureServiceTokenProvider1 = new AzureServiceTokenProvider();
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider1.KeyVaultTokenCallback));

            // Optional: Request an access token to other Azure services
            //var azureServiceTokenProvider2 = new AzureServiceTokenProvider();

            //var accessTokenTask = azureServiceTokenProvider2.GetAccessTokenAsync("ffe3cbcf-8945-489c-a8e6-bc73995bbfcb");

            //accessTokenTask.Wait();
            //var accessToken = accessTokenTask.Result;
            var azureServiceTokenProvider2 = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider2.GetAccessTokenAsync("https://targetwebapi.azurewebsites.net").ConfigureAwait(false);

            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = httpclient.GetAsync("https://targetwebapi.azurewebsites.net/api/values").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return Ok(content);
            }
            return BadRequest(response.ReasonPhrase + Environment.NewLine + accessToken);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "staging-value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
