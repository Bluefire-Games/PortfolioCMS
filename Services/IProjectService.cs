using PortfolioCMS.Models;

namespace PortfolioCMS.Services
{
    public interface IProjectService
    {

        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectAsync(int id);
        Task CreateProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task<List<Project>> GetFilteredProjectsAsync(string? keyword, string? sortOrder, int? categoryId = null, List<int>? tagIds = null);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Tag>> GetAllTagsAsync();
        Task UpdateProjectMetadataAsync(Project project);
        Task UpdateProjectTagsAsync(int projectId, List<Tag> allTagsFromUI);

    }
}
