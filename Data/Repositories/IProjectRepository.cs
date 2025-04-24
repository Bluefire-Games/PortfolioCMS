using PortfolioCMS.Models;

namespace PortfolioCMS.Data.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task<List<Project>> GetAllAsync();
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);

    }
}
