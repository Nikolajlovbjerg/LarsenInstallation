using Core;
using System.Net.Http.Json;

namespace Client.Service
{
    public class ProjectService
    {
        private readonly HttpClient _http;

        public ProjectService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Project?> CreateProject(Project project)
        {
            var response = await _http.PostAsJsonAsync("api/project", project);

            return await response.Content.ReadFromJsonAsync<Project>();
        }

        public async Task<List<Project>> GetProjects()
        {
            return await _http.GetFromJsonAsync<List<Project>>("api/project");
        }
    }
}