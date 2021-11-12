import { MessageEmbed } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';

export const run: RunFunction = async(client, message, args) => {
   
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    var embededMessage = new MessageEmbed()
            .addField("Help commands", `${guild.setting.prefix}help-reddit \n ${guild.setting.prefix}help-mods \n ${guild.setting.prefix}help-playlists \n ${guild.setting.prefix}help-settings \n ${guild.setting.prefix}help-bans`)
            .setColor("DARK_GOLD")
            .setTimestamp();

            message.channel.send(embededMessage);
};


export const name: string = 'Help';
export const alias: string = 'Help';