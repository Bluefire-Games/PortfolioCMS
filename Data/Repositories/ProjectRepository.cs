using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Models;


namespace PortfolioCMS.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PortfolioDbContext _context;

        public ProjectRepository(PortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllAsync() =>
            await _context.Projects.Include(p => p.Images).ToListAsync();

        public async Task<Project?> GetByIdAsync(int id) =>
            await _context.Projects.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
