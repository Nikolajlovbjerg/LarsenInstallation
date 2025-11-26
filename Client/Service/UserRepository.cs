using Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Service
{
    public class UserRepository
    {
        private readonly HttpClient _http;

        public UserRepository(HttpClient http)
        {
            _http = http;
        }
        
        public async Task<Users?> ValidLoginAsync(string name, string password)
        {
            var login = new { UserName = name, Password = password };
            HttpResponseMessage response;

            try
            {
                response = await _http.PostAsJsonAsync("/api/user/login", login);
            }
            catch (Exception)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<Users>();
                return user;
            }
            
            return null;
        }
    }
}