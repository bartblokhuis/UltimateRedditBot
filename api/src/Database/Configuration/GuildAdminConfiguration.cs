using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration
{
    public class GuildAdminConfiguration : IEntityTypeConfiguration<GuildAdmin>
    {
        public void Configure(EntityTypeBuilder<GuildAdmin> builder)
        {
            builder.HasOne(x => x.Guild)
                .WithMany(x => x.GuildAdmins);

            builder.HasOne(x => x.User)
                .WithMany(x => x.GuildAdmins);
        }
    }
}
