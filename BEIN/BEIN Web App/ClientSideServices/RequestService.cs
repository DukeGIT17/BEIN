using BEIN_Web_App.IClientSideServices;
using System.Text.Json;
using System.Text;

namespace BEIN_Web_App.ClientSideServices
{
    public class RequestService(HttpClient client, IConfiguration configuration) : IRequestService
    {
        private readonly string BasePath = $"{configuration["APIBasePaths:Https"]}";
        private readonly Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> GetRequestAsync<T>(string endpoint)
        {
            try
            {
                var response = await client.GetAsync(BasePath + endpoint);
                if (!response.IsSuccessStatusCode) throw new($"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync()}");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = await response.Content.ReadFromJsonAsync<T>();
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetRequestAsync(string endpoint)
        {
            try
            {
                var response = await client.GetAsync(BasePath + endpoint);
                if (!response.IsSuccessStatusCode) throw new($"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync()}");

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> SendRequestAsync<T>(T model, HttpMethod method, string endpoint)
        {
            try
            {
                var response = await client.SendAsync(new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
                    Method = method,
                    RequestUri = new Uri(BasePath + endpoint)
                });

                if (!response.IsSuccessStatusCode) throw new($"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync()}");

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
