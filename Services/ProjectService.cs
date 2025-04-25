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
            return await _db.Projects
                .Include(p => p.Images)
                .Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectAsync(int id)
        {
            return await _db.Projects
                .Include(p => p.Images)
                .Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateProjectAsync(Project project)
        {
            if (project.CategoryId != 0)
            {
                project.Category = await _db.Categories.FindAsync(project.CategoryId);
            }

            if (project.ProjectTags != null && project.ProjectTags.Any())
            {
                foreach (var pt in project.ProjectTags)
                {
                    _db.Attach(pt.Tag);
                }
            }

            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project updatedProject)
        {
            var existingProject = await _db.Projects
                .Include(p => p.Images)
                .Include(p => p.ProjectTags)
                .FirstOrDefaultAsync(p => p.Id == updatedProject.Id);

            if (existingProject == null) return;

            // Scalar fields
            existingProject.Title = updatedProject.Title;
            existingProject.Description = updatedProject.Description;
            existingProject.RepoLink = updatedProject.RepoLink;
            existingProject.CategoryId = updatedProject.CategoryId;

            existingProject.ProjectTags.Clear();
            foreach (var pt in updatedProject.ProjectTags)
            {
                existingProject.ProjectTags.Add(new ProjectTag
                {
                    ProjectId = updatedProject.Id,
                    TagId = pt.TagId
                });
            }

            // ✅ Add new images only (skip duplicates)
            foreach (var img in updatedProject.Images)
            {
                if (!existingProject.Images.Any(ei => ei.Path == img.Path))
                {
                    existingProject.Images.Add(new ProjectImage { Path = img.Path });
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateProjectMetadataAsync(Project updatedProject)
        {
            var existingProject = await _db.Projects.FirstOrDefaultAsync(p => p.Id == updatedProject.Id);
            if (existingProject == null) return;

            existingProject.Title = updatedProject.Title;
            existingProject.Description = updatedProject.Description;
            existingProject.RepoLink = updatedProject.RepoLink;
            existingProject.CategoryId = updatedProject.CategoryId;

            // Add new images
            foreach (var img in updatedProject.Images)
            {
                if (!_db.ProjectImages.Any(ei => ei.ProjectId == existingProject.Id && ei.Path == img.Path))
                {
                    existingProject.Images.Add(new ProjectImage { Path = img.Path });
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateProjectTagsAsync(int projectId, List<Tag> allTagsFromUI)
        {
            var project = await _db.Projects
                .Include(p => p.ProjectTags)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null) return;

            var selectedTagIds = allTagsFromUI
                .Where(t => t.IsSelected)
                .Select(t => t.Id)
                .ToHashSet();

            // Get current tag IDs
            var currentTagIds = project.ProjectTags.Select(pt => pt.TagId).ToHashSet();

            // Tags to remove
            var tagsToRemove = project.ProjectTags.Where(pt => !selectedTagIds.Contains(pt.TagId)).ToList();

            // Tags to add
            var tagsToAdd = selectedTagIds.Except(currentTagIds);

            foreach (var tagToAdd in tagsToAdd)
            {
                project.ProjectTags.Add(new ProjectTag { TagId = tagToAdd, ProjectId = projectId });
            }

            foreach (var tagToRemove in tagsToRemove)
            {
                project.ProjectTags.Remove(tagToRemove);
            }

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

        public async Task<List<Project>> GetFilteredProjectsAsync(string? keyword, string? sortOrder, int? categoryId = null, List<int>? tagIds = null)
        {
            var query = _db.Projects
                .Include(p => p.Images)
                .Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.Title.Contains(keyword) || p.Description.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (tagIds != null && tagIds.Count > 0)
            {
                query = query.Where(p => p.ProjectTags.Any(pt => tagIds.Contains(pt.TagId)));
            }

            query = sortOrder switch
            {
                "title_asc" => query.OrderBy(p => p.Title),
                "title_desc" => query.OrderByDescending(p => p.Title),
                "date_asc" => query.OrderBy(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            return await query.ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _db.Tags.OrderBy(t => t.Name).ToListAsync();
        }
    }
}
