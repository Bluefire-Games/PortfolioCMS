using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Project> Projects { get; set; } = new();
    }
}
