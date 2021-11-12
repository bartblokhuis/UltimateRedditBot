﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(UltimateRedditBotDbContext))]
    [Migration("20210919194911_playlists_add_guild_fk")]
    partial class playlists_add_guild_fk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0-rc.1.21452.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.BannedSubreddit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GuildId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubredditId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("SubredditId");

                    b.ToTable("BannedSubreddits");
                });

            modelBuilder.Entity("Domain.Entities.ChannelSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GuildChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GuildChannelId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("ChannelSubscriptions");
                });

            modelBuilder.Entity("Domain.Entities.Guild", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiscordGuildId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GuildSettingId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Domain.Entities.GuildAdmin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GuildId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("GuildAdmins");
                });

            modelBuilder.Entity("Domain.Entities.GuildChannel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiscordChannelId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GuildId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("GuildChannels");
                });

            modelBuilder.Entity("Domain.Entities.GuildSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AllowNsfw")
                        .HasColumnType("bit");

                    b.Property<Guid>("GuildId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GuildId")
                        .IsUnique();

                    b.ToTable("GuildSettings");
                });

            modelBuilder.Entity("Domain.Entities.Playlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("GuildId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsGlobal")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOver18")
                        .HasColumnType("bit");

                    b.Property<string>("Permalink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Selftext")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Domain.Entities.PostHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GuildId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastPostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.Property<Guid>("SubredditId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("SubredditId");

                    b.ToTable("PostHistories");
                });

            modelBuilder.Entity("Domain.Entities.Subreddit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNsfw")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subreddits");
                });

            modelBuilder.Entity("Domain.Entities.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastPostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.Property<Guid>("SubredditId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubredditId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiscordUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlaylistSubreddit", b =>
                {
                    b.Property<Guid>("PlaylistsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubredditsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlaylistsId", "SubredditsId");

                    b.HasIndex("SubredditsId");

                    b.ToTable("PlaylistSubreddit");
                });

            modelBuilder.Entity("Domain.Entities.BannedSubreddit", b =>
                {
                    b.HasOne("Domain.Entities.Guild", "Guild")
                        .WithMany("BannedSubreddits")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Subreddit", "Subreddit")
                        .WithMany()
                        .HasForeignKey("SubredditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Subreddit");
                });

            modelBuilder.Entity("Domain.Entities.ChannelSubscription", b =>
                {
                    b.HasOne("Domain.Entities.GuildChannel", "GuildChannel")
                        .WithMany("ChannelSubscriptions")
                        .HasForeignKey("GuildChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Subscription", "Subscription")
                        .WithMany("ChannelSubscriptions")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GuildChannel");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Domain.Entities.GuildAdmin", b =>
                {
                    b.HasOne("Domain.Entities.Guild", "Guild")
                        .WithMany("GuildAdmins")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("GuildAdmins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.GuildChannel", b =>
                {
                    b.HasOne("Domain.Entities.Guild", "Guild")
                        .WithMany("GuildChannels")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Domain.Entities.GuildSetting", b =>
                {
                    b.HasOne("Domain.Entities.Guild", "Guild")
                        .WithOne("GuildSetting")
                        .HasForeignKey("Domain.Entities.GuildSetting", "GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Domain.Entities.Playlist", b =>
                {
                    b.HasOne("Domain.Entities.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Domain.Entities.PostHistory", b =>
                {
                    b.HasOne("Domain.Entities.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Subreddit", "Subreddit")
                        .WithMany()
                        .HasForeignKey("SubredditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Subreddit");
                });

            modelBuilder.Entity("Domain.Entities.Subscription", b =>
                {
                    b.HasOne("Domain.Entities.Subreddit", "Subreddit")
                        .WithMany()
                        .HasForeignKey("SubredditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subreddit");
                });

            modelBuilder.Entity("PlaylistSubreddit", b =>
                {
                    b.HasOne("Domain.Entities.Playlist", null)
                        .WithMany()
                        .HasForeignKey("PlaylistsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Subreddit", null)
                        .WithMany()
                        .HasForeignKey("SubredditsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Guild", b =>
                {
                    b.Navigation("BannedSubreddits");

                    b.Navigation("GuildAdmins");

                    b.Navigation("GuildChannels");

                    b.Navigation("GuildSetting")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.GuildChannel", b =>
                {
                    b.Navigation("ChannelSubscriptions");
                });

            modelBuilder.Entity("Domain.Entities.Subscription", b =>
                {
                    b.Navigation("ChannelSubscriptions");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("GuildAdmins");
                });
#pragma warning restore 612, 618
        }
    }
}
