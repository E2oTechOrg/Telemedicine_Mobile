using System.Text;
using System.Text.Json;
using telemedicine.Models;

namespace telemedicine.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress =
                new Uri("https://app-bgm-hospital-b4hbefbzd4ffbhhj.canadacentral-01.azurewebsites.net/");
        }

        public async Task<T?> PostAsync<T>(string url, object data)
        {
            var json = JsonSerializer.Serialize(data);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response =
                await _httpClient.PostAsync(url, content);

            string result =
                await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine(result);

            try
            {
                return JsonSerializer.Deserialize<T>(
                    result,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"JSON Error: {ex.Message}\n\nResponse:\n{result}");
            }
        }

        public async Task PutAsync(string url, object data)
        {
            var json = JsonSerializer.Serialize(data);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response =
                await _httpClient.PutAsync(url, content);

            string result =
                await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine(result);

            response.EnsureSuccessStatusCode();
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(
                result,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task SaveFcmTokenAsync(
      int doctorId,
      string fcmToken)
        {
            var request = new DeviceTokenRequest
            {
                DoctorId = doctorId,
                FcmToken = fcmToken
            };

            await PostAsync<object>(
                "api/Appointment/save-token",
                request);
        }

    }
}