using ChartServer.Data.Configuration;
using ChartServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChartServer.Data
{
    public class EFContext : DbContext
    {
        public DbSet<StatRecord> Records { get; set; }

        public EFContext(DbContextOptions opts) : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new StatRecordConfiguration());
        }
    }
}
