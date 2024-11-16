using Branef.Empresas.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Branef.Empresas.DB.EntityConfigurations.ReadDb
{
    public class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToCollection("companies");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired(true).HasConversion<string>().ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255).HasColumnName("name").IsUnicode(true);
            builder.Property(x => x.Size).HasColumnName("size").HasConversion<string>();
            builder.Property(x => x.IsDeleted).HasDefaultValue(false).HasColumnName("deleted");

            builder.Property(x => x.IncludedAt).HasBsonRepresentation(MongoDB.Bson.BsonType.DateTime).HasColumnName("included_at");
            builder.Property(x => x.UpdatedAt).HasBsonRepresentation(MongoDB.Bson.BsonType.DateTime).HasColumnName("updated_at");
            builder.Property(x => x.DeletedAt).HasBsonRepresentation(MongoDB.Bson.BsonType.DateTime).HasColumnName("deleted_at");
        }
    }
}
