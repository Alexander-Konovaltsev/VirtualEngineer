using VirtualEngineer.Models;
using VirtualEngineer.Enums;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace VirtualEngineer.Services
{
    public class ApiService
    {
        private const string BaseUrl = "http://127.0.0.1:8080";

        public static async Task<Role[]> GetRoles()
        {
            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(BaseUrl + Endpoint.Roles);

                if (!response.IsSuccessStatusCode)
                    return null;

                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Role[]>(json);
            }
            catch
            {
                return null;
            }
        }
        
        public static async Task<UserCreateResult> CreateUser(UserCreateRequest user)
        {
            using var client = new HttpClient();

            string json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(BaseUrl + Endpoint.UserCreate, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return UserCreateResult.Success;

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    return UserCreateResult.EmailAlreadyExists;

                return UserCreateResult.NetworkError;
            }
            catch (TaskCanceledException)
            {
                return UserCreateResult.TimeoutError;
            }
            catch
            {
                return UserCreateResult.NetworkError;
            }
        }
    }
}
