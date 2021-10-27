using Ibis_CSR_Tool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Ibis_CSR_Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuspendUserController : ControllerBase
    {
        private readonly ILogger<SuspendUserController> _logger;
        private readonly IConfiguration _configuration;

        public SuspendUserController(ILogger<SuspendUserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("suspenduser")]
        public object SuspendUser(object data, string result, string API_Key)
        {
            try
            {
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

                        var uri = "https://webappsdev.iltest.illinois.gov/DES/CSRTool/api/SuspendUser/suspenduser"; 

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

                    /* Check if Unlink is completed */
                    if ((obj != null)) 
                    //&& String.IsNullOrEmpty(obj.ibis_id))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id + "/lifecycle/suspend");
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Authorization", "SSWS" + API_Key);
                        var body = @"";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                       
                        /* Get user details */
                         var _client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id);
                        _client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        _client.Timeout = -1;
                        var _request = new RestRequest(Method.GET);
                        _request.AddHeader("Accept", "application/json");
                        _request.AddHeader("Content-Type", "application/json");
                        _request.AddHeader("Authorization", "SSWS" + API_Key);
                        IRestResponse _response = _client.Execute(_request);
                        result = _response.Content;
                        obj = System.Text.Json.JsonSerializer.Deserialize<LinkUser>(result.ToString());
                    }
                else
                {
                   result = @"{""errorCode"":""E0000009"", ""errorSummary"":""Please try again later""}"; 
                }
            }
        }
            catch (System.Exception ex)
            {
                result = @"{""errorCode"":""E0000009"", ""errorSummary"":""" + ex.ToString() + @"""}";
                //throw;
            }
            return result;
        }

        [HttpPost]
        [Route("unsuspenduser")]
        public object UnSuspendUser(object data, string result, string API_Key)
        {
            try
            {
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

                        var uri = "https://webappsdev.iltest.illinois.gov/DES/CSRTool/api/SuspendUser/unsuspenduser"; 

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

                     /* Check if Unlink is completed */
                    if ((obj != null)) 
                    //&& String.IsNullOrEmpty(obj.ibis_id))
                    {
                        var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id + "/lifecycle/unsuspend");
                        client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Authorization", "SSWS" + API_Key);
                        var body = @"";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                       
                        /* Get user details */
                         var _client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id);
                        _client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                        _client.Timeout = -1;
                        var _request = new RestRequest(Method.GET);
                        _request.AddHeader("Accept", "application/json");
                        _request.AddHeader("Content-Type", "application/json");
                        _request.AddHeader("Authorization", "SSWS" + API_Key);
                        IRestResponse _response = _client.Execute(_request);
                        result = _response.Content;
                        //obj = System.Text.Json.JsonSerializer.Deserialize<LinkUser>(result.ToString());
                    }
                else
                {
                   result = @"{""errorCode"":""E0000009"", ""errorSummary"":""Please try again later""}"; 
                }
            }
        }
            catch (System.Exception ex)
            {
                result = @"{""errorCode"":""E0000009"", ""errorSummary"":""" + ex.ToString() + @"""}";
                //throw;
            }
            return result;
        }

                
                   /* var client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id + "/lifecycle/unsuspend");
                    client.Proxy = new WebProxy("http://webgateway.illinois.gov:9090");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Authorization", "SSWS" + API_Key);
                    var body = @"";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                
                    //Get User and return user response
                    if (obj.Id != null) {
                        var _client = new RestClient(OktaDomain + "/api/v1/users/" + obj.Id);
                        _client.Timeout = -1;
                        var _request = new RestRequest(Method.GET);
                        _request.AddHeader("Accept", "application/json");
                        _request.AddHeader("Content-Type", "application/json");
                        _request.AddHeader("Authorization", "SSWS" + API_Key);
                        IRestResponse _response = _client.Execute(_request);
                        results = _response.Content;
                    }
                }
            }
            catch (System.Exception ex)
            {
                results = @"{""errorCode"":""E0000009"", ""errorSummary"":""" + ex.ToString() + @"""}";
                //throw;
            }
           return results;
        }*/
    }
}

