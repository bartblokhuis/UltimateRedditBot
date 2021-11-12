using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration
{
    class ChannelSubscriptionConfiguration : IEntityTypeConfiguration<ChannelSubscription>
    {
        public void Configure(EntityTypeBuilder<ChannelSubscription> builder)
        {
            builder
                .HasOne<GuildChannel>()
                .WithMany()
                .HasForeignKey(x => x.GuildChannelId);

            builder.HasOne<Subscription>()
                .WithMany()
                .HasForeignKey(x => x.SubscriptionId);
        }
    }
}
