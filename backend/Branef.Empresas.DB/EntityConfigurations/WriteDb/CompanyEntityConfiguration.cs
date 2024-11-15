using Branef.Empresas.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Branef.Empresas.DB.EntityConfigurations.WriteDb
{
    public class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("companies");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255).HasColumnName("name");
            builder.Property(x => x.Size).HasConversion<string>();

            builder.Property(x => x.IsDeleted).HasDefaultValue(false).HasColumnName("deleted");
            builder.Property(x => x.IncludedAt).HasDefaultValue(DateTimeOffset.UtcNow).HasColumnName("included_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");
        }
    }
}
