using Ibis_CSR_Tool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ibis_CSR_Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnlinkUserController : ControllerBase
    {
        private readonly ILogger<UnlinkUserController> _logger;
        private readonly IConfiguration _configuration;

        public UnlinkUserController(ILogger<UnlinkUserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("unlinkfromibis")]
        public object UnLinkUserFromIbis(object data, string result, string API_Key)
        {
           // var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
            var baseUrl = "https://webappsdev.iltest.illinois.gov/DES/CSRTool";

            /* Post Packet containing UserId, FirstName, LastName, Email, Login, DriversLicense and IbisID 
            From the Frontend*/

            bool unlinked = false;
            LinkUser obj = System.Text.Json.JsonSerializer.Deserialize<LinkUser>(data.ToString());

            if (obj != null)
            {
                var uri = baseUrl + "/api/UnlinkUser/unlinkuserfromibis";

                using (var httpClient = new HttpClient())
                {

                    var req = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(uri)
                    };

                }

                var ibisLinkAPI = _configuration.GetSection("Okta").GetSection("ibisUnLinkAPI").Value;
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
                        unlinked =   true;
                    }
                    else
                    {
                        unlinked =   false;
                    }
                }
                else
                {
                    unlinked =   false;
                }
            }

            return unlinked;
        }
    }
}
