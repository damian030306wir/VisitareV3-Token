using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Visitare.Services
{
    class Services
    {
        public async Task<string> LoginAsync(string username, string password)
        {

            string URL = "http://dearjean.ddns.net:44301/Token";
            var accessToken = string.Empty;
            await Task.Run(() =>
            {
                try
                {
                    var keyValues = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Username", username),
                        new KeyValuePair<string, string>("Password", password),
                        new KeyValuePair<string, string>("grant_type", "password")
                    };
                    var request = new HttpRequestMessage(
                        HttpMethod.Post, URL);

                    request.Content = new FormUrlEncodedContent(keyValues);

                    var client = new HttpClient();
                    var response = client.SendAsync(request).Result;


                    using (HttpContent content = response.Content)
                    {
                        var json = content.ReadAsStringAsync();
                        JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(json.Result);
                        var accessTokenExpiration = jwtDynamic.Value<DateTime>(".expires");
                        accessToken = jwtDynamic.Value<string>("access_token");

                        var Username = jwtDynamic.Value<String>("userName");
                        var AccessTokenExpirationDate = accessTokenExpiration;

                    }
                }

                catch (Exception e)
                {

                }
            });
            return accessToken;

        }
    }
}
