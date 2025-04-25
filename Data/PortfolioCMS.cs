using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Models;
using System.Collections.Generic;

namespace PortfolioCMS.Data
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProjectTag> ProjectTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectTag>()
                .HasKey(pt => pt.Id);

            modelBuilder.Entity<ProjectTag>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTags)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProjectTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}
