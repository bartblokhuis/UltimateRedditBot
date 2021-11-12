import { RunFunction } from '../../interfaces/Command';
import { TextChannel } from 'discord.js';
import { SubredditService } from '../../services/SubredditService';
import Container from 'typedi';
import { GuildService } from '../../services/GuildService';
import { DiscordUtils } from '../../utils/discordUtils';
import { SubscriptionService } from '../../services/SubscriptionService';

export const run: RunFunction = async(client, message, args) => {
    if(!args || args.length === 0){
        message.channel.send('Please provide a subreddit');
        return;
    }

    if(!message.channel.isText()){
        return;
    }

    const guildService = Container.get(GuildService)
    const guild = await guildService.getGuild(message.guild.id);

    if(!await DiscordUtils.isAdmin(message)){
        message.channel.send("Only mods can unban subreddits");
        return;
    }

    const channel = message.channel as TextChannel;

    const subredditName = args[0].toString();
    const subRedditService = Container.get(SubredditService);

    const subreddit = await subRedditService.getByName(subredditName);
    if(!subreddit){
        message.channel.send("Subreddit doesn't exist");
        return;
    }

    subRedditService.unBanSubreddit(subreddit.id, guild.id)
        .then((result) => {
            const subscriptionService = Container.get(SubscriptionService);
            subscriptionService.unbannedSubreddit(subreddit.name, guild.id);
            channel.send(result.messages[0]);
        })
        .catch((error) => {
            console.log(error);
            channel.send("Error, please try again");
        })
    
};

export const name: string = 'unban';
export const alias: string = 'unban';