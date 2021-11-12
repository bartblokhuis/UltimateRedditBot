import { MessageEmbed } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';

export const run: RunFunction = async(client, message, args) => {
   
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    var embededMessage = new MessageEmbed()
        .addField("Enabled", `Usage: ${guild.setting.prefix}enabled (true/false) \n mods can enable and disable the bot using this command. When the bot is disabled non of the commands will do anything except for the command to enable the bot.`)
        .addField("Allow nsfw", `Usage: ${guild.setting.prefix}enabled (true/false) \n When turned off no nsfw marked posts will be send to the server, when this setting is turned on it will only send nsfw marked posts in nsfw channels`)
        .addField("Prefix", `Usage: ${guild.setting.prefix}prefix (new prefix) \n Used to change the bots prefix`)
        .addField("Sort", `Usage: ${guild.setting.prefix}sort (new sorting method) \n Used to change the default sorting method, the following sorting methods are supported: \n 1. Hot \n 2. New \n 3. Random \n 4. topnow \n 5. toptody \n 6. topthisweek \n 7. topthismonth \n 8. topthisyear \n 9. topalltime`)
        .setColor("DARK_GOLD")
        .setTimestamp();

            message.channel.send(embededMessage);
};


export const name: string = 'Help-settings';
export const alias: string = 'Help-settings';