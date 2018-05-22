using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using RestSharp;
using RestSharp.Authenticators;
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
        //const string redirectUri = "http://localhost:54320/home/oauth2callback";
        AuthorizationServerDescription server = new AuthorizationServerDescription
        {
            AuthorizationEndpoint = new Uri("https://webapi.developers.erstegroup.com/api/csas/sandbox/v1/sandbox-idp/authorize"),
            TokenEndpoint = new Uri("https://webapi.developers.ersteapihub.com/api/csas/sandbox/v1/sandbox-idp/token"),
            ProtocolVersion = ProtocolVersion.V20,
        };

        public ActionResult Index()
        {
            //var client = new RestClient("https://api.developer.rb.cz/psd2-rbcz-sandbox-oauth2-api"); // RAIFFKA sandbox
            //var client = new RestClient("https://api.developer.rb.cz/oauth2/authorize"); // RAIFFKA ostrý
            var client = new RestClient("https://webapi.developers.erstegroup.com/api/csas/sandbox/v1/sandbox-idp"); // ČS náhradní
            client.CookieContainer = new System.Net.CookieContainer();
            //client.Authenticator = new ("client_id", "9049865d-cdb3-4b26-9b30-066d7a05f0a7", "response_type", "code");
            var request = new RestRequest("authorize?state={state}&response_type={response_type}&client_id={client_id}&access_type={access_type}&approval_prompt={approval_prompt}", Method.GET);
            request.AddParameter("state", "csas-auth", ParameterType.UrlSegment);
            request.AddParameter("response_type", "code", ParameterType.UrlSegment);
            //request.AddParameter("scope", "all", ParameterType.UrlSegment);
            request.AddParameter("client_id", clientID, ParameterType.UrlSegment);
            request.AddParameter("access_type", "online", ParameterType.UrlSegment);
            request.AddParameter("approval_prompt", "force", ParameterType.UrlSegment);
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            //var request_token = new RestRequest("token?grand_type={grand_type}&code={code}&client_id={client_id}&client_secret={client_secret}", Method.POST);
            var request_token = new RestRequest("token", Method.POST);
            //request.AddHeader("web-api-key", "b6ed089e-4679-4ce2-ac6a-84895d914720");
            request_token.AddParameter("grand_type", "authorization_code", ParameterType.RequestBody);
            request_token.AddParameter("code", "test-code", ParameterType.RequestBody);
            request_token.AddParameter("client_id", clientID, ParameterType.RequestBody);
            request_token.AddParameter("client_secret", clientSecret, ParameterType.RequestBody);
            IRestResponse response_token = client.Execute(request_token);
            content = response_token.Content; // raw content as string

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