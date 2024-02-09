using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using technicalevaluation.Models;

namespace technicalevaluation.Data.Map
{
    public class UnitMap : IEntityTypeConfiguration<UnitInfo>
    {
        public void Configure(EntityTypeBuilder<UnitInfo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.UnitId).IsRequired();
        }
    }
}
