import { MessageEmbed } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';

export const run: RunFunction = async(client, message, args) => {
   
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    var embededMessage = new MessageEmbed()
            .addField("Description", "Mods can change settings, modify playlists, subscribe or unsubscribe and ban / unban subreddits")
            .addField("Usage", `${guild.setting.prefix}mod @username \n ${guild.setting.prefix}unmod @username`)
            .setColor("DARK_GOLD")
            .setTimestamp();

            message.channel.send(embededMessage);
};


export const name: string = 'Help-mods';
export const alias: string = 'Help-mods';