using VirtualEngineer.Models;
using VirtualEngineer.Enums;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace VirtualEngineer.Services
{
    public static class ApiService
    {
        public const string BaseUrl = "http://127.0.0.1:8080";
        
        public static async Task<Role[]> GetRoles()
        {
            string url = BaseUrl + Endpoint.Roles;

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return JsonConvert.DeserializeObject<Role[]>(request.downloadHandler.text);
                }

                return null;
            }
        }
    }
}
