using Ibis_CSR_Tool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    public class IdProofingController : ControllerBase
    {
        private readonly ILogger<IdProofingController> _logger;
        private readonly IConfiguration _configuration;
        public IdProofingController(ILogger<IdProofingController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("IdProofParameters")]
        public object IdProofUser(object data, string result, string _result, string API_Key)
        {
            try
            {
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                IdProofUser obj = System.Text.Json.JsonSerializer.Deserialize<IdProofUser>(data.ToString());
                if ((obj != null) && (obj.Id != null))
                {
                    var uri = "https://webappsdev.iltest.illinois.gov/DES/CSRTool/api/IdProofing/IdProofParameters";
                    var proxy = new WebProxy
                    {
                        Address = new Uri($"http://webgateway.illinois.gov:9090"),
                        BypassProxyOnLocal = false,
                    };
                    var httpClientHandler = new HttpClientHandler
                    {
                        Proxy = proxy,
                        UseProxy = true,
                    };
                    {
                        using (var httpClient = new HttpClient(httpClientHandler))
                        {
                            var req = new HttpRequestMessage
                            {
                                Method = HttpMethod.Post,
                                RequestUri = new Uri(uri)
                            };
                        }
                    }
                    var OktaDomain = _configuration.GetSection("Okta").GetSection("OktaDomain").Value;
                    API_Key = _configuration.GetSection("Okta").GetSection("API Key").Value;

                    var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id);
                    client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "SSWS" + API_Key);
                    
                    var body = @"{" + "\n" +
                    @"    ""profile"": {" + "\n" +
                    @"        ""idproofing_LOA"":""4""" + "," + "\n" +
                    @"        ""idproofing_score"":""""" + "," + "\n" +
                    @"        ""idproofing_status"":""ibis_override""" + "\n" +
                    @"  }" + "\n" +
                    @"}";
                    ///*---------------------------------------------------*/
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    result = response.Content;
                }
            }
            catch (System.Exception ex)
            {
                result = @"{""errorCode"":""E0000009"", ""errorSummary"":""" + ex.ToString() + @"""}";
                //throw;
            }            
            return result;
        }
    }
}
