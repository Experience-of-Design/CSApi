using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIClient.API
{
    public class ApiRBController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        const string BaseUrl = "https://api.twilio.com/2008-08-01";

        readonly string _accountSid;
        readonly string _secretKey;

        public ApiRBController(string accountSid, string secretKey)
        {
            _accountSid = accountSid;
            _secretKey = secretKey;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new System.Uri(BaseUrl);
            client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response.Data;
        }

        // TwilioApi.cs, method of TwilioApi class
        public Call GetCall(string callSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Calls/{CallSid}";
            request.RootElement = "Call";

            request.AddParameter("CallSid", callSid, ParameterType.UrlSegment);

            return Execute<Call>(request);
        }

        //public Call InitiateOutboundCall() // CallOptions options
        //{
            //Require.Argument("Caller", options.Caller);
            //Require.Argument("Called", options.Called);
            //Require.Argument("Url", options.Url);

            //var request = new RestRequest(Method.POST);
            //request.Resource = "Accounts/{AccountSid}/Calls";
            //request.RootElement = "Calls";

            //request.AddParameter("Caller", options.Caller);
            //request.AddParameter("Called", options.Called);
            //request.AddParameter("Url", options.Url);

            //if (options.Method.HasValue) request.AddParameter("Method", options.Method);
            //if (options.SendDigits.HasValue()) request.AddParameter("SendDigits", options.SendDigits);
            //if (options.IfMachine.HasValue) request.AddParameter("IfMachine", options.IfMachine.Value);
            //if (options.Timeout.HasValue) request.AddParameter("Timeout", options.Timeout.Value);

            //return Execute<Call>(request);
        //}
    }


    public class Call
    {
        public string Sid { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CallSegmentSid { get; set; }
        public string AccountSid { get; set; }
        public string Called { get; set; }
        public string Caller { get; set; }
        public string PhoneNumberSid { get; set; }
        public int Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public int Flags { get; set; }
    }
}