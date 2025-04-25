using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace PortfolioCMS.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string RepoLink { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<ProjectImage> Images { get; set; } = new();

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<ProjectTag> ProjectTags { get; set; } = new();
    }



}
