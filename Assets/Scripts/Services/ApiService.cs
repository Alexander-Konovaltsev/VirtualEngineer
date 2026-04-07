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

                if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    return UserCreateResult.DataError;

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

        public static async Task<UserAuthorizationResult> AuthorizationUser(UserAuthorizationRequest auth)
        {
            using var client = new HttpClient();

            string json = JsonConvert.SerializeObject(auth);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(BaseUrl + Endpoint.UserAuthorization, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<UserAuthorizationResponse>(responseJson);

                    SessionService.SetToken(data.access_token);

                    return UserAuthorizationResult.Success;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return UserAuthorizationResult.InvalidCredentials;

                return UserAuthorizationResult.NetworkError;
            }
            catch (TaskCanceledException)
            {
                return UserAuthorizationResult.TimeoutError;
            }
            catch
            {
                return UserAuthorizationResult.NetworkError;
            }
        }
    }
}
