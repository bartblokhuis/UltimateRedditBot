import { MessageEmbed } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';

export const run: RunFunction = async(client, message, args) => {
   
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    var embededMessage = new MessageEmbed()
            .addField("Description", "Mods have the ability to ban or unban a subreddit from being used")
            .addField("Usage", `${guild.setting.prefix}ban (subreddit) \n ${guild.setting.prefix}unban (subreddit)`)
            .setColor("DARK_GOLD")
            .setTimestamp();

            message.channel.send(embededMessage);
};


export const name: string = 'Help-bans';
export const alias: string = 'Help-bans';