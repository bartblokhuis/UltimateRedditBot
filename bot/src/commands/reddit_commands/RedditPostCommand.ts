import { RunFunction } from '../../interfaces/Command';
import { TextChannel } from 'discord.js';
import { SubredditService } from '../../services/SubredditService';
import { RedditService } from '../../services/RedditService';
import { Container } from 'typedi';
import { QueueService } from '../../services/QueueService';
import { GuildService } from '../../services/GuildService';
import { Sort } from '../../data/Sort';

export const run: RunFunction = async(client, message, args) => {
    if(!args || args.length === 0){
        message.channel.send('Please provide a subreddit');
        return;
    }

    let amountOfPosts = 1;
    if(args.length > 1){
        var argAmountOfPosts = parseInt(args[1]);
        if(argAmountOfPosts) amountOfPosts = argAmountOfPosts;
    }

    if(!message.channel.isText()){
        return;
    }

    const channel = message.channel as TextChannel;

    const subredditName = args[0].toString();
    const subRedditService = Container.get(SubredditService);

    const subreddit = await subRedditService.getByName(subredditName);

    if(!subreddit){
        channel.send("Subreddit doesn't exist");
        return;
    }
    
    if(subreddit.isNsfw && !channel.nsfw){
        channel.send("You can't use nsfw subreddits in non nsfw channels");
        return;
    }

    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    if(subreddit.isNsfw && !guild.setting.allowNsfw){
        channel.send("The server has turned off nsfw subreddits.");
        return;
    }

    let redditSort : Sort = guild.setting.sort;

    if(args.length > 1 && isNaN(Number(args[1]))){
        let mayBeSort: Sort | undefined = (<any>Sort)[args[1].toLowerCase()];
            if (mayBeSort !== undefined) redditSort = mayBeSort;
    }

    if(await subRedditService.isSubredditBanned(guild.id, subreddit.id)){
        channel.send("This subreddit is banned from the server.");
        return;
     }

    const queueService = Container.get(QueueService);
    queueService.addToQueue(subreddit, channel, amountOfPosts, guild, redditSort, message.author.username);
};

export const name: string = 'reddit';
export const alias: string = 'r';