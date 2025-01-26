using ClinicTrialApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicTrialApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

        public DbSet<ClinicalTrial> ClinicalTrials { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClinicalTrial>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key
                entity.Property(e => e.TrialId)
                      .IsRequired()
                      .HasMaxLength(50); // Trial ID column is required and has a max length of 50
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(255); // Title is required with a max length of 255
                entity.Property(e => e.StartDate)
                      .IsRequired();
                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(50);
            });
        }*/
    }
}
