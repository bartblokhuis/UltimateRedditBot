using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration
{
    public class GuildConfiguration : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.HasOne(a => a.GuildSetting)
                .WithOne(a => a.Guild)
                .HasForeignKey<GuildSetting>(c => c.GuildId);

            builder.HasMany(a => a.BannedSubreddits)
                .WithOne(a => a.Guild);

            builder.HasMany(a => a.GuildAdmins)
                .WithOne(a => a.Guild);
        }
    }
}
