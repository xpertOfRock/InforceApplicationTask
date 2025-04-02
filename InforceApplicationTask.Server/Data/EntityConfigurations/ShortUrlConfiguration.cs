using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InforceApplicationTask.Server.Data.EntityConfigurations
{
    public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
    {
        public void Configure(EntityTypeBuilder<ShortUrl> builder)
        {
            builder
                .HasIndex(s => s.ShortCode)
                .IsUnique();

            builder
                .Property(s => s.ShortCode)
                .HasMaxLength(10);
        }
    }
}
