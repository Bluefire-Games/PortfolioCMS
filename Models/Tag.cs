using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioCMS.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<ProjectTag> ProjectTags { get; set; } = new();

        [NotMapped]
        public bool IsSelected { get; set; }  // Used in UI binding
    }



}
