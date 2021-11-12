using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration
{
    public class BannedSubredditConfiguration : IEntityTypeConfiguration<BannedSubreddit>
    {
        public void Configure(EntityTypeBuilder<BannedSubreddit> builder)
        {
            builder.HasOne(x => x.Guild)
                .WithMany(x => x.BannedSubreddits);
        }
    }
}
