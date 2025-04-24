using PortfolioCMS.Models;

namespace PortfolioCMS.Services
{
    public interface IProjectService
    {

        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectAsync(int id);
        Task<List<Project>> GetFilteredProjectsAsync(string? keyword, string? sortOrder);
        Task CreateProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
    }
}
