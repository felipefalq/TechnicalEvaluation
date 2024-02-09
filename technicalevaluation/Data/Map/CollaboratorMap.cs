using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace technicalevaluation.Data.Map
{
    public class CollaboratorMap : IEntityTypeConfiguration<CollaboratorInfo>
    {
        public void Configure(EntityTypeBuilder<CollaboratorInfo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            
            builder.Ignore(x => x.Unit);
        }
    }
}
