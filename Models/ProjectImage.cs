namespace PortfolioCMS.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;

        public int ProjectId { get; set; }
        public Project Project { get; set; } = default!;
    }

}
