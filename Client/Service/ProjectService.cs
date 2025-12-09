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

        // --- DETTE ER METODEN DU MANGLER ---
        // Denne metode ringer til din backend (ProjectCalculationsService)
        // og henter de færdige beregninger.
        public async Task<Calculation?> GetProjectDetails(int id)
        {
            try
            {
                // Kalder endpointet: api/project/5 (hvis id er 5)
                return await _http.GetFromJsonAsync<Calculation>($"api/project/{id}");
            }
            catch (Exception)
            {
                // Hvis projektet ikke findes eller der er fejl, returner null
                return null;
            }
        }
        // -----------------------------------
    }
}