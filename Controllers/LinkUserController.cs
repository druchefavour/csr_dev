using Ibis_CSR_Tool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace Ibis_CSR_Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkUserController : ControllerBase
    {
        private readonly ILogger<LinkUserController> _logger;
        private readonly IConfiguration _configuration;
        public LinkUserController(ILogger<LinkUserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /*[HttpGet]
        [Route("getgroups")]
        public string[] GetGroups(string[] result)
        {
            //List Groups to get the Group IDs
            var client = new RestClient("https://dev-41732283.okta.com/api/v1/groups");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "SSWS 00ZqJN7_u8OgrxQhh12lKyx7hwopZ7cImh9GW-US1B");
            request.AddHeader("Cookie", "DT=DI0V6ZxpjfZSe-PzN2WCDH0lw; JSESSIONID=8190536A901C68DE7B05089A0877A0B0; sid=102fDyn28baS3i9qoQwLyhqsg; t=default");
            IRestResponse response = client.Execute(request);

            var groupResponse = response.Content;

            List<GroupClass> groupData = JsonConvert.DeserializeObject<List<GroupClass>>(groupResponse.ToString());

            foreach (var c in groupData)
            {
                result = new string[] { c.id, c.profile.name };
            }

            var array = new List<GroupClass>().ToArray();
            foreach (var c in groupData)
            {
            }
            return result;
        }*/

        [HttpPost]
        [Route("linktoibis")]
        public object LinkUserToIbis(object data, string result, string _result, string ___result, string API_Key, string users)
        {
            //var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";

            var baseUrl = "https://webappsdev.iltest.illinois.gov/DES/CSRTool";
            // Post Id, Ibis Request Group ID and IbisID
            bool linked = false;
            LinkUser obj = System.Text.Json.JsonSerializer.Deserialize<LinkUser>(data.ToString());
            if (obj != null)
            {
                var uri = baseUrl + "/api/LinkUser/linktoibis";

                using (var httpClient = new HttpClient())
                {

                    var req = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(uri)
                    };

                }

                var ibisLinkAPI = _configuration.GetSection("Okta").GetSection("ibisLinkAPI").Value;
                var ibisAPIKey = _configuration.GetSection("Okta").GetSection("ibisAPIKey").Value;
                var client = new RestClient(ibisLinkAPI);
                client.Timeout = -1;
                client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "Basic " + ibisAPIKey);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("cache-control", "no-cache");
                var body = @"{""ibisUsername"": """ + obj.ibis_id + @""",""oktaUsername"": """ + obj.Id + @""",
                ""csrLoginUsername"": """ + obj.csrUsername + @""",""oktaLoginUsername"": """ + obj.Login + @"""}";
                request.AddParameter("application/json", body,  ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string responsebody = response.Content.ToString().ToLower();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (responsebody.Equals("true"))
                    {
                        linked =   true;
                    }
                    else
                    {
                        linked =   false;
                    }
                }
                else
                {
                    linked =   false;
                }
            }
            return linked;
        }
    }
}
