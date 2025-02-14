using BEIN_DL.Models;
using BEIN_Web_App.IClientSideServices;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using static BEIN_Web_App.ClientSideServices.Helper;

namespace BEIN_Web_App.ClientSideServices
{
    public class RequestService(HttpClient client, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IRequestService
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

                var token = httpContextAccessor.HttpContext!.Request.Cookies["AuthToken"];
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

        public async Task<Dictionary<string, object>> SignInRequestAsync(SignInModel model)
        {
            try
            {
                var response = await client.SendAsync(new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(BasePath + "/Account/SignIn")
                });

                if (!response.IsSuccessStatusCode) throw new($"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync()}");

                string token = "";
                if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
                {
                    var authHeader = cookieHeaders.FirstOrDefault(c => c.StartsWith("AuthToken="));
                    if (!string.IsNullOrEmpty(authHeader))
                        token = authHeader.Replace("AuthToken=", "");
                }

                if (await response.Content.ReadFromJsonAsync<List<ClaimDto>>() is not List<ClaimDto> claims) throw new("Could not retrieve claims from the API.");
                
                var claimsIdentity = new ClaimsIdentity(claims.Select(c => new Claim(c.Type, c.Value)), "Cookies");
                await httpContextAccessor.HttpContext!.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

                if (string.IsNullOrEmpty(token)) throw new("Failed to retrieve authentication token");
                client.DefaultRequestHeaders.Authorization = new("Bearer", token);

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

        public async Task<Dictionary<string, object>> SignOutRequestAsync()
        {
            try
            {
                var response = await client.GetAsync(BasePath + "/Account/SignOut");
                if (!response.IsSuccessStatusCode) throw new(response.ReasonPhrase);
                await httpContextAccessor.HttpContext!.SignOutAsync("Cookies");
                client.DefaultRequestHeaders.Authorization = null;

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

        public async Task<Dictionary<string, object>> SendFileAsync(IFormFile file, HttpMethod method, string endpoint)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                AttachFileToMPFD(file, formData);

                var response = await client.SendAsync(new HttpRequestMessage
                {
                    Content = formData,
                    Method = method,
                    RequestUri = new Uri(BasePath + endpoint),

                });

                if (!response.IsSuccessStatusCode) throw new($"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync()}");

                _returnDictionary["Success"] = true;
                _returnDictionary["FileName"] = await response.Content.ReadAsStringAsync();
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
