import Container from 'typedi';
import { Sort } from '../../data/Sort';
import { RunFunction } from '../../interfaces/Command';
import { GuildChannelService } from '../../services/GuildChannelService';
import { GuildService } from '../../services/GuildService';
import { SubredditService } from '../../services/SubredditService';
import { SubscriptionApiService } from '../../services/SubscriptionApiService';
import { SubscriptionService } from '../../services/SubscriptionService';
import { DiscordUtils } from '../../utils/discordUtils';

export const run: RunFunction = async(client, message, args) => {
    
    const guildService = Container.get(GuildService)
    const guild = await guildService.getGuild(message.guild.id);

    if(!await DiscordUtils.isAdmin(message)){
        message.channel.send("Only mods can unsubscribe");
        return;
    }

    if(!args || args.length === 0){
        message.channel.send("Please enter a subreddit");
        return;
    }

    const subredditName = args[0].toString();
    const subRedditService = Container.get(SubredditService);

    const subreddit = await subRedditService.getByName(subredditName);

    if(!subreddit){
        message.channel.send("Subreddit doesn't exist");
        return;
    }
    
    const channelService = Container.get(GuildChannelService)
    const apiChannel = await channelService.getChannel(guild.id, message.channel.id);
    
    const subscriptionApiService = Container.get(SubscriptionApiService);
    
    subscriptionApiService.unSubscribe(subreddit.id, Sort.new, apiChannel.id)
    .then((result) => {
        if(result.succeeded){
            const subscriptionService = Container.get(SubscriptionService);
            subscriptionService.removeSubscriptionChannel(result.data, apiChannel.id)
            message.channel.send(`This channel is no longer subscribed to: ${subreddit.name}`);
        }
        else{
            message.channel.send(result.messages[0]);
        }
    });
};

export const name: string = 'ubsubscribe';
export const alias: string = 'unsub';