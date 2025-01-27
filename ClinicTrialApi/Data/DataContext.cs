using ClinicTrialApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicTrialApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

        public DbSet<ClinicalTrial> ClinicalTrials { get; set; }
    }
}
