using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Models;
using System.Collections.Generic;

namespace PortfolioCMS.Data
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }

    }
}
