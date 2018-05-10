using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIClient.Controllers
{
    public class HomeController : Controller
    {
        const string clientID = "sandboxClientId";
        const string clientSecret = "sandboxClientSecret";
        const string redirectUri = "http://localhost:54320/home/oauth2callback";
        AuthorizationServerDescription server = new AuthorizationServerDescription
        {
            AuthorizationEndpoint = new Uri("https://webapi.ersteapihub.com/api/csas/sandbox/v1/sandbox-idp/auth"),
            TokenEndpoint = new Uri("https://webapi.ersteapihub.com/api/csas/sandbox/v1/sandbox-idp/token"),
            ProtocolVersion = ProtocolVersion.V20,
        };

        public ActionResult Index()
        {
            var client = new RestClient("https://webapi.ersteapihub.com/api/csas/sandbox/v1/sandbox-idp/auth");
            var request = new RestRequest(Method.GET);
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            //request.AddHeader("web-api-key", "b6ed089e-4679-4ce2-ac6a-84895d914720");
            request.AddQueryParameter("state", "csas-auth");
            request.AddQueryParameter("response_type", "code");
            request.AddQueryParameter("client_id", "sandboxClientId");
            request.AddQueryParameter("access_type", "online");
            request.AddQueryParameter("approval_prompt", "force");
            //request.AddParameter("application/x-www-form-urlencoded", "state=csas-auth&client_id=sandboxClientId&response_type=code&access_type=online&approval_prompt=force", ParameterType.RequestBody); // redirect_uri=http://localhost:54320&
            IRestResponse response = client.Execute(request);


            //List<string> scope = new List<string> { };
            //WebServerClient consumer = new WebServerClient(server, clientID, clientSecret);
            //// Here redirect to authorization site occurs

            ////var token = consumer.ExchangeUserCredentialForToken(clientID, clientSecret);
            //OutgoingWebResponse response = consumer.PrepareRequestUserAuthorization(
            //    scope, new Uri(redirectUri));
            //response.Headers.Add("web-api-key", "b6ed089e-4679-4ce2-ac6a-84895d914720");
            //consumer.RequestUserAuthorization(scope, new Uri("http://localhost:54320/home/oauth2callback"));
            //return response.AsActionResultMvc5();



            return Content(response.Content);
        }
        public ActionResult oauth2callback()
        {
            WebServerClient consumer = new WebServerClient(server, clientID, clientSecret);
            consumer.ClientCredentialApplicator =
                ClientCredentialApplicator.PostParameter(clientSecret);
            IAuthorizationState grantedAccess = consumer.ProcessUserAuthorization(null);

            string accessToken = grantedAccess.AccessToken;
            return Content(accessToken);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}