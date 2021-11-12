import { RunFunction } from '../../interfaces/Command';
import Container from 'typedi';
import { GuildService } from '../../services/GuildService';
import { PlaylistService } from '../../services/PlaylistService';
import { TextChannel } from 'discord.js';
import { QueueService } from '../../services/QueueService';
import { Sort } from '../../data/Sort';

export const run: RunFunction = async(client, message, args) => {

    if(!args || args.length === 0){
        message.channel.send('Please provide a name');
        return;
    }

    const name = capitalize(args[0]);

    const playListService = Container.get(PlaylistService);
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    const playList = await playListService.getByName(name, guild.id);
    if(!playList){
        message.channel.send("Couldn't find playlist");
        return;
    }

    if(playList.subreddits.length === 0){
        message.channel.send(`There are no subreddits in the playlist, you can add a playlist using the command: $p-add ${name} (subreddit)`);
        return;
    }

    const channel = message.channel as TextChannel;
    if(!channel.nsfw || !guild.setting.allowNsfw){
        playList.subreddits = playList.subreddits.filter(subreddit => !subreddit.isNsfw);

        if(playList.subreddits.length === 0){
            message.channel.send(`There are no non nsfw subreddits in the playlist, you can add a playlist using the command: $p-add ${name} (subreddit)`);
            return;
        }
    }

    const randomIndex = Math.floor(Math.random() * playList.subreddits.length);
    const subreddit = playList.subreddits[randomIndex];

    const queueService = Container.get(QueueService);
    queueService.addToQueue(subreddit, channel, 1, guild, Sort.hot, message.author.username);

    function capitalize(str: string): string{
        const lower = str.toLocaleLowerCase();
        return str.charAt(0).toUpperCase() + lower.slice(1);
    }
};


export const name: string = 'playlist';
export const alias: string = 'p';