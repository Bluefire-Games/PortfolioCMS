using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.Models;


namespace PortfolioCMS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly PortfolioDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProjectService(PortfolioDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _db.Projects.Include(p => p.Images).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Project?> GetProjectAsync(int id)
        {
            return await _db.Projects.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var proj = await _db.Projects.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            if (proj == null) return;

            foreach (var img in proj.Images)
            {
                var fullPath = Path.Combine(_env.WebRootPath, img.Path.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }

            _db.Projects.Remove(proj);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Project>> GetFilteredProjectsAsync(string? keyword, string? sortOrder)
        {
            var query = _db.Projects.Include(p => p.Images).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.Title.Contains(keyword) || p.Description.Contains(keyword));
            }

            query = sortOrder switch
            {
                "title_asc" => query.OrderBy(p => p.Title),
                "title_desc" => query.OrderByDescending(p => p.Title),
                "date_asc" => query.OrderBy(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt), // default: newest first
            };

            return await query.ToListAsync();
        }
    }



}
