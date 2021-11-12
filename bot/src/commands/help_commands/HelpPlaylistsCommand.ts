import { MessageEmbed } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';

export const run: RunFunction = async(client, message, args) => {
   
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    var embededMessage = new MessageEmbed()
        .addField("Description", "Playlists can be used to create a group of subreddits and request a post from one the subreddits in the groupd")
        .addField("Post", `Usage: ${guild.setting.prefix}p (playlist name) Get a reddit post`)
        .addField("Playlists",  `Usage: ${guild.setting.prefix}p-info \n Used to see the playlists in the server`)
        .addField("Subreddits in playlist", `Usage: ${guild.setting.prefix}p-info (playlist name) \n Used to see the subreddits in a playlist`)
        .addField("Add playlist", `Usage: ${guild.setting.prefix}p-add (name) \n mods can use this command to create a new playlist.`)
        .addField("Add subreddit to playlist", `Usage: ${guild.setting.prefix}p-add (playlist name) (subreddit name) \n Mods can use this command to add a subreddit to a plsylist.`)
        .addField("Remove playlist", `Usage: ${guild.setting.prefix}p-remove (name) \n Mods can use this command to remove a plsylist`)
        .addField("Remove subreddit from playlist", `Usage: ${guild.setting.prefix}p-remove (playlist name) (subreddit name) \n Mods can use this command to remove a subreddit from a playlist`)
        .setColor("DARK_GOLD")
        .setTimestamp();

            message.channel.send(embededMessage);
};


export const name: string = 'Help-playlists';
export const alias: string = 'Help-p';