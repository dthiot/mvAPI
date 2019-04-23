using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlueFinity.mvNET.CoreObjects;
using mvNetApi.Filters;
using mvNetApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace mvNetApi.Controllers
{
    public class MvNetApiController : ApiController
    {
        //// GET: mvNetApi
        //[HttpGet]
        //public string Get()
        //{
        //    return "Get";
        //}

        // GET: mvNetApi
        [HttpGet]
        [MyBasicAuthenticationFilter]
        public HttpResponseMessage Get()
        {
            try
            {
                int am = 254;
                int vm = 253;
                char charAM = (char)am;
                var charVM = (char)vm;

                var subName = "";
                var subParams = "";

                var qstring = this.Request.GetQueryNameValuePairs();
                foreach (KeyValuePair<string, string> rpp in qstring)
                {
                    var rppKey = rpp.Key;
                    if (rppKey == "SP")
                    {
                        subName = rpp.Value;
                    }
                    else
                    {
                        if (subParams != "") subParams = subParams + charAM;
                        subParams = subParams + rppKey + charVM + rpp.Value;
                    }
                }

                if (subName != "")
                {
                    string tlsRequest = ConfigurationManager.AppSettings["TlsRequest"];
                    if (tlsRequest == "true")
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    }

                    var requestSubName = "";

                    requestSubName = ConfigurationManager.AppSettings["mvNetSubPrefix"] + subName;
                    var requestParams = subParams;
                    var myResponse = "";

                    var mvAccount = new mvAccount(ConfigurationManager.AppSettings["mvNetLogin"]);

                    try
                    {
                        mvAccount.CallProg(requestSubName.ToString(), ref requestParams, ref myResponse);

                        mvAccount.Logout();

                        DynApiResponse jsonResponse = new DynApiResponse
                        {
                            ApiResponse = JsonConvert.DeserializeObject<dynamic>(myResponse)
                        };

                        var execSpResponse = Request.CreateResponse(HttpStatusCode.OK, jsonResponse);
                        return execSpResponse;
                    }
                    catch (Exception ex)
                    {
                        var errmsg = ex.Message;
                        errmsg = errmsg.Substring(0, 36);
                        if (mvAccount.Connected)
                        {
                            mvAccount.Logout();
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.GatewayTimeout, ex.Message);
                        }
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.GatewayTimeout, "Error Encounters");
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // POST: mvNetApi
        [HttpPost]
        [MyBasicAuthenticationFilter]
        public HttpResponseMessage ExecSp([FromBody] MvNetApiRequest request)
        {
            try
             {
                if (request != null)
                {
                    string tlsRequest = ConfigurationManager.AppSettings["TlsRequest"];
                    if (tlsRequest == "true")
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    }

                    var requestSubName = "";

                    requestSubName = ConfigurationManager.AppSettings["mvNetSubPrefix"] + request.SubName;
                    var requestParams = request.Params;
                    var myResponse = "";

                    var mvAccount = new mvAccount(ConfigurationManager.AppSettings["mvNetLogin"]);

                    try
                    {
                        mvAccount.CallProg(requestSubName.ToString(), ref requestParams, ref myResponse);

                        mvAccount.Logout();

                        DynApiResponse jsonResponse = new DynApiResponse
                        {
                            ApiResponse = JsonConvert.DeserializeObject<dynamic>(myResponse)
                        };

                        var execSpResponse = Request.CreateResponse(HttpStatusCode.OK, jsonResponse);
                        return execSpResponse;
                    }
                    catch (Exception ex)
                    {
                        var errmsg = ex.Message;
                        errmsg = errmsg.Substring(0, 36);
                        if (mvAccount.Connected)
                        {
                            mvAccount.Logout();
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.GatewayTimeout, ex.Message);
                        }
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.GatewayTimeout, "Error Encounters");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse  (HttpStatusCode.InternalServerError , ex.Message  );
            }
        }
    }
}

