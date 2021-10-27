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
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ibis_CSR_Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorsController : ControllerBase
    {
        private readonly ILogger<FactorsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FactorsController(ILogger<FactorsController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("listfactors")]
        [HttpGet]
        [Route("factors")]
        public object ListFactor(object data, string result, string API_Key)
        {
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
            LinkUser obj = System.Text.Json.JsonSerializer.Deserialize<LinkUser>(data.ToString());
            if (obj != null)
            {
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

                var uri = "https://webappsdev.iltest.illinois.gov/DES/CSRTool/api/Factors/listfactors";

                using (var _httpClient = new HttpClient(httpClientHandler))
                {

                    var req = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(uri)
                    };
                }

                var OktaDomain = _configuration.GetSection("Okta").GetSection("OktaDomain").Value;
                API_Key = _configuration.GetSection("Okta").GetSection("API Key").Value;

                var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id + "/factors");
                client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization",  "SSWS" + API_Key);
                IRestResponse response = client.Execute(request);

                result = response.Content;
            }
            return result;
        }

        [HttpPost]
        [Route("clearfactors")]
        public object ClearFactor(object data, string result, string resultSms,  string resultEm, 
        string resultGa, string resultOkVer, string resultOkVerT, string resultSq, string resultVc, string API_Key)
        {
            try
            {
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
                MFAFactor obj = System.Text.Json.JsonSerializer.Deserialize<MFAFactor>(data.ToString());
                if (obj != null)
                {
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

                    var uri = "https://webappsdev.iltest.illinois.gov/DES/CSRTool/api/Factors/clearfactors"; 

                    using (var httpClient = new HttpClient(httpClientHandler))
                    {

                        var req = new HttpRequestMessage
                        {
                            Method = HttpMethod.Post,
                            RequestUri = new Uri(uri)
                        };
                    }
                
                    var OktaDomain = _configuration.GetSection("Okta").GetSection("OktaDomain").Value;
                    API_Key = _configuration.GetSection("Okta").GetSection("API Key").Value;

                    /* If all factor types selected call Reset All*/


                    /* If few factors selected, Reset Factors one after the other */
                    /* Reset SMS */
                    if (!String.IsNullOrEmpty(obj.Idsms))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.UserId + "/factors/" + obj.Idsms);
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.DELETE);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "SSWS " + API_Key);
                        var _body = @"";
                        request.AddParameter("application/json", _body, ParameterType.RequestBody);
                        IRestResponse _response = client.Execute(request);
                        resultSms = _response.Content;
                    }
                    if (!String.IsNullOrEmpty(obj.Idem))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.UserId + "/factors/" + obj.Idem);
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.DELETE);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "SSWS " + API_Key);
                        var _body = @"";
                        request.AddParameter("application/json", _body, ParameterType.RequestBody);
                        IRestResponse _response = client.Execute(request);
                        resultEm = _response.Content;
                    }
                    if (!String.IsNullOrEmpty(obj.IdGA))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.UserId + "/factors/" + obj.IdGA);
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.DELETE);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "SSWS " + API_Key);
                        var _body = @"";
                        request.AddParameter("application/json", _body, ParameterType.RequestBody);
                        IRestResponse _response = client.Execute(request);
                        resultGa = _response.Content;
                    }
                    if (!String.IsNullOrEmpty(obj.Idokver))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.UserId + "/factors/" + obj.Idokver);
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.DELETE);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "SSWS " + API_Key);
                        var _body = @"";
                        request.AddParameter("application/json", _body, ParameterType.RequestBody);
                        IRestResponse _response = client.Execute(request);
                        resultOkVer = _response.Content;
                    }
                    if (!String.IsNullOrEmpty(obj.Idokvert))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.UserId + "/factors/" + obj.Idokvert);
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.DELETE);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "SSWS " + API_Key);
                        var _body = @"";
                        request.AddParameter("application/json", _body, ParameterType.RequestBody);
                        IRestResponse _response = client.Execute(request);
                        resultOkVerT = _response.Content;
                    }
                    if (!String.IsNullOrEmpty(obj.Idsq))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.UserId + "/factors/" + obj.Idsq);
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.DELETE);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "SSWS " + API_Key);
                        var _body = @"";
                        request.AddParameter("application/json", _body, ParameterType.RequestBody);
                        IRestResponse _response = client.Execute(request);
                        resultSq = _response.Content;
                    }
                    if (!String.IsNullOrEmpty(obj.Idvc))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.UserId + "/factors/" + obj.Idvc);
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.DELETE);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "SSWS " + API_Key);
                        var _body = @"";
                        request.AddParameter("application/json", _body, ParameterType.RequestBody);
                        IRestResponse _response = client.Execute(request);
                        resultVc = _response.Content;
                    }
                    result = @"{""statusCode"":""200""}";

                }
                else
                {
                    result = @"{""errorCode"":""E0000009"", ""errorSummary"":""An unexpected error has occured""}";
                }
            }
            catch (System.Exception ex)
            {
                result = @"{""errorCode"":""E0000009"", ""errorSummary"":""" + ex.ToString() + @"""}";
                //throw;
            }            
            // return new string[] {result, resultSms, resultEm, resultGa, resultOkVer, resultOkVerT, resultSq, resultVc};
            return result;
        }
    }
}
