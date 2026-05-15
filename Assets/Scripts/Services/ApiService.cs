using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System;
using System.Net;

namespace VirtualEngineer.Services
{
    public class ApiService
    {
        private const string BaseUrl = "http://127.0.0.1:8080";

        public static async Task<ApiResponse<T[]>> GetAsync<T>(
            string endpoint, 
            bool isProtected = true
        )
        {
            using var client = new HttpClient();

            if (isProtected)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", 
                    SessionService.AccessToken
                );
            }

            try
            {
                var response = await client.GetAsync(BaseUrl + endpoint);

                string json = await response.Content.ReadAsStringAsync();

                T[] data = Array.Empty<T>();

                if (!string.IsNullOrWhiteSpace(json))
                {
                    data = JsonConvert.DeserializeObject<T[]>(json) ?? Array.Empty<T>();
                }

                return new ApiResponse<T[]>
                {
                    isSuccess = response.IsSuccessStatusCode,
                    statusCode = response.StatusCode,
                    data = data
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<T[]>
                {
                    isSuccess = false,
                    statusCode = HttpStatusCode.RequestTimeout,
                    data = Array.Empty<T>()
                };
            }
            catch (Exception)
            {
                return new ApiResponse<T[]>
                {
                    isSuccess = false,
                    statusCode = HttpStatusCode.InternalServerError,
                    data = Array.Empty<T>()
                };
            }
        }

        public static async Task<ApiResponse<TResult>> PostAsync<TRequest, TResult>(
            string endpoint, 
            TRequest data,
            bool isProtected = true
        )
        {
            using var client = new HttpClient();

            if (isProtected)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", 
                    SessionService.AccessToken
                );
            }

            string json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(BaseUrl + endpoint, content);

                string responseJson = await response.Content.ReadAsStringAsync();

                TResult result = default;

                if (!string.IsNullOrWhiteSpace(responseJson))
                {
                    result = JsonConvert.DeserializeObject<TResult>(responseJson);
                }

                return new ApiResponse<TResult>
                {
                    isSuccess = response.IsSuccessStatusCode,
                    statusCode = response.StatusCode,
                    data = result
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<TResult>
                {
                    isSuccess = false,
                    statusCode = HttpStatusCode.RequestTimeout,
                    data = default
                };
            }
            catch (Exception)
            {
                return new ApiResponse<TResult>
                {
                    isSuccess = false,
                    statusCode = HttpStatusCode.InternalServerError,
                    data = default
                };
            }
        }
    }
}
