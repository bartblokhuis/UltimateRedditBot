import { MessageEmbed } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';

export const run: RunFunction = async(client, message, args) => {
   
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    var embededMessage = new MessageEmbed()
        .addField("Description", "This command can be used to get a or multiple posts from a subreddit")
        .addField("Usage", `**${guild.setting.prefix}(subreddit name) (sorting method) (amount of posts)** \n The sorting method and the amount of posts are both optional. 
            The default sorting method is configurable in the settings, the default amount of posts is 1`)
        .setColor("DARK_GOLD")
        .setTimestamp();

            message.channel.send(embededMessage);
};


export const name: string = 'Help-reddit';
export const alias: string = 'Help-r';