using AutoMapper;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.AutoMapper
{
    public class DiscordAutoMapperProfile: Profile
    {
        public DiscordAutoMapperProfile()
        {
            CreateMap<Guild, GuildDto>();
            CreateMap<GuildDto, Guild>();
        }
    }
}
