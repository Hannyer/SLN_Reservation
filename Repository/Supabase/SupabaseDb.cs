using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Repository.Supabase
{
    public static class SupabaseRest
    {
        private static readonly HttpClient _http = new HttpClient();
        private static readonly string _url;
        private static readonly string _key;

        static SupabaseRest()
        {
            _url = System.Configuration.ConfigurationManager.AppSettings["SUPABASE_URL"];
            _key = System.Configuration.ConfigurationManager.AppSettings["SUPABASE_KEY"];
            _http.BaseAddress = new Uri($"{_url}/rest/v1/");
            _http.DefaultRequestHeaders.Add("apikey", _key);
            _http.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _key);
        }

        public static async Task<T> PostRpcAsync<T>(string functionName, object payload)
        {
            var relative = $"rpc/{functionName}";
            var requestUri = _http.BaseAddress + relative;
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine("POST to: " + requestUri);
            Debug.WriteLine("Body: " + json);
            var response = await _http.PostAsync(relative, content);
            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
            Debug.WriteLine("Response Body: " + body);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"RPC '{functionName}' falló ({(int)response.StatusCode}): {body}"
                );
            }
            return JsonConvert.DeserializeObject<T>(body);
        }
    }
}
