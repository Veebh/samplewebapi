using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using corewebapiwithswaggerui;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace samplewebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        //[Authorize(Roles = Role.Admin)]
        public ActionResult<string> Get()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            var keyvalue = new List<KeyValuePair<string, string>>();
            if (null != principal)
            {
                if (principal.Claims == null)
                {
                    keyvalue.Add(new KeyValuePair<string, string>("principal.Claims", $"principal.Claims is null"));
                }
                else
                {
                    keyvalue.Add(new KeyValuePair<string, string>("principal.Claims.Count", principal.Claims.ToList<Claim>().Count.ToString()));

                }
                foreach (Claim claim in principal.Claims)
                {
                    keyvalue.Add(new KeyValuePair<string, string>(claim.Type, claim.Value.ToString()));
                }
            }
            else
            {
                keyvalue.Add(new KeyValuePair<string, string>("missing claims", $"principle is null"));
            }
            if (HttpContext.User.Identity != null)
            {
                keyvalue.Add(new KeyValuePair<string, string>("HttpContext.User.Identity.AuthenticationType", HttpContext.User.Identity.AuthenticationType));
                keyvalue.Add(new KeyValuePair<string, string>("HttpContext.User.Identity.IsAuthenticated", HttpContext.User.Identity.IsAuthenticated.ToString()));
                keyvalue.Add(new KeyValuePair<string, string>("HttpContext.User.Identity.Name", HttpContext.User.Identity.Name));
            }
            try
            {
                Claim displayName = ClaimsPrincipal.Current?.FindFirst(ClaimsPrincipal.Current.Identities?.First()?.NameClaimType);
                keyvalue.Add(new KeyValuePair<string, string>("displayName", displayName != null ? displayName.Value : string.Empty));
            }
            catch (Exception ex)
            {
                keyvalue.Add(new KeyValuePair<string, string>("corewebapiwithswaggerui-headers", ex.Message + ex.StackTrace));
            }
            //ControllerBase.User
            //keyvalue.Add(new KeyValuePair<string, string>(" ControllerBase.User", ControllerBase.User.Claims);
            foreach (Claim claim in this.User.Claims)
            {
                keyvalue.Add(new KeyValuePair<string, string>(" ControllerBase.User " + claim.Type, claim.Value.ToString()));
            }
            if (this.User.Identity != null)
            {
                keyvalue.Add(new KeyValuePair<string, string>(" ControllerBase.User HttpContext.User.Identity.AuthenticationType", this.User.Identity.AuthenticationType));
                keyvalue.Add(new KeyValuePair<string, string>(" ControllerBase.User HttpContext.User.Identity.IsAuthenticated", this.User.Identity.IsAuthenticated.ToString()));
                keyvalue.Add(new KeyValuePair<string, string>(" ControllerBase.User HttpContext.User.Identity.Name", this.User.Identity.Name));
            }
            keyvalue.Add(new KeyValuePair<string, string>("Enduser jwt header", "------------------------------------------"));
            var jwtReader = CustomJwtValidation.CustomValidateJwt(HttpContext);
            if (jwtReader == null)
            {
                keyvalue.Add(new KeyValuePair<string, string>("Request Header is null ", "------------------------------------------"));

            }
            else
            {
                foreach (var h in jwtReader.JwtInHeader)
                {
                    keyvalue.Add(new KeyValuePair<string, string>(h.Key, h.Value.ToString()));

                }

                keyvalue.Add(new KeyValuePair<string, string>("Enduser Clams", "------------------------------------------"));
                foreach (var c in jwtReader.JwtCliams)
                {
                    keyvalue.Add(new KeyValuePair<string, string>(c.Type, c.Value));

                }
                keyvalue.Add(new KeyValuePair<string, string>("Request claims", "------------------------------------------"));
            }

            foreach (var header in HttpContext.Request.Headers)
            {
                var key = header.Key;
                var val = header.Value;
                keyvalue.Add(new KeyValuePair<string, string>(key, val));
            }
            keyvalue.Add(new KeyValuePair<string, string>("appname", "corewebapiwithswaggerui"));
            return JsonConvert.SerializeObject(keyvalue, Newtonsoft.Json.Formatting.Indented).ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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

    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
