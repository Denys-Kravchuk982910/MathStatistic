using ChartServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChartServer.Data.Configuration
{
    public class StatRecordConfiguration : IEntityTypeConfiguration<StatRecord>
    {
        public void Configure(EntityTypeBuilder<StatRecord> builder)
        {
            builder.ToTable("tblRecords");

            builder.HasKey(k => k.Id);
        }
    }
}
