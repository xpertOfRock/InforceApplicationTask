using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InforceApplicationTask.Server.Data.EntityConfigurations
{
    public class AboutConfiguration : IEntityTypeConfiguration<About>
    {
        public void Configure(EntityTypeBuilder<About> builder)
        {
            builder
                .Property(x => x.Description)
                .HasMaxLength(600);
        }
    }
}
