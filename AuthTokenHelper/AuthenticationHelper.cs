using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AuthTokenHelper
{
    public class AuthenticationHelper
    {
        public static TokenResponse GetBasicAuthSignInToken(string tokenurl, string username, string password)
        {
            TokenResponse response = new TokenResponse();
            string responseToken = GetBasicToken(tokenurl, username, password);
            JObject jobj = JObject.Parse(responseToken);
            string token = (string)jobj["access_token"];  
            if (token == null)
            {
                string error = (string)jobj["error"];
                string description = (string)jobj["error_description"];
                response.accessToken = "error:" + error + " ,error description : " + description;
                response.isSuccess = false;
            }
            else
            {
                response.accessToken = token;                
                response.expiresIn = (string)jobj["expires_in"];
                response.isSuccess = true;
            }
            return response;
        }

        static string GetBasicToken(string url, string username, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "grant_type", "client_credentials")
            };

            var content = new FormUrlEncodedContent(pairs);
            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var response = client.PostAsync(url, content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }

        }

    }
}
