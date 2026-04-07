using VirtualEngineer.Models;
using VirtualEngineer.Enums;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine;

namespace VirtualEngineer.Services
{
    public class ApiService
    {
        private const string BaseUrl = "http://127.0.0.1:8080";
        private const int TimeoutTime = 5;
        
        public static async Task<Role[]> GetRoles()
        {
            string url = BaseUrl + Endpoint.Roles;

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();
                float startTime = Time.time;

                while (!operation.isDone)
                {
                    if (IsTimeout(startTime))
                    {
                        request.Abort();
                        return null;
                    }

                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return JsonConvert.DeserializeObject<Role[]>(request.downloadHandler.text);
                }

                return null;
            }
        }

        public static async Task<UserCreateResult> CreateUser(UserCreateRequest user)
        {
            string url = BaseUrl + Endpoint.UserCreate;

            string json = JsonConvert.SerializeObject(user);

            using (UnityWebRequest request = UnityWebRequest.Post(url, json, "application/json"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
    
                var operation = request.SendWebRequest();
                float startTime = Time.time;

                while (!operation.isDone)
                {
                    Debug.Log("WAITING...");
                    if (IsTimeout(startTime))
                    {
                        request.Abort();
                        return UserCreateResult.TimeoutError;
                    }

                    await Task.Yield();
                }
                Debug.Log("EXIT...");
                long status = request.responseCode;

                if (status == 201)
                {
                    return UserCreateResult.Success;
                }

                if (status == 409)
                {
                    return UserCreateResult.EmailAlreadyExists;
                }

                return UserCreateResult.NetworkError;
            }
        }

        private static bool IsTimeout(float startTime)
        {
            return Time.time - startTime > TimeoutTime;
        }
    }
}
