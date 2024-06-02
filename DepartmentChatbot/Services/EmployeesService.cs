using DepartmentChatbot.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DepartmentChatbot.Services
{
    public class EmployeesService
    {
        const string uriMain = App.uri + "WmiIEmployes";
        readonly HttpClient httpClient;

        public EmployeesService()
        {
            this.httpClient = new HttpClient();
            Task task = Task.Run(async () =>
            {
                await InitializeHttpClient();
            });
            task.Wait();
        }
        async Task InitializeHttpClient()
        {
            string? deviceId = await SecureStorage.GetAsync("deviceId");
            string? apikey = await SecureStorage.GetAsync("apikey");
            httpClient.DefaultRequestHeaders.Add("device_id", deviceId);
            httpClient.DefaultRequestHeaders.Add("api_key", apikey);
        }

        async public Task<ObservableCollection<Employee>> GetEmployees()
        {
            var response = await httpClient.GetAsync(uriMain);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(responseContent);
            return list;
        }
    }
}
