using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration
{
    public class GuildSettingConfiguration : IEntityTypeConfiguration<GuildSetting>
    {
        public void Configure(EntityTypeBuilder<GuildSetting> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(e => e.Guild)
                .WithOne(e => e.GuildSetting);
        }
    }
}
