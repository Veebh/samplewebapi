using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace samplewebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
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

            var response = httpclient.GetAsync("https://targetwebapi.azurewebsites.net/api/values").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return content;
            }
            return response.ReasonPhrase;
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
