import { Client, TextChannel } from "discord.js";
import { Service } from "typedi";
import { Subscription } from "../data/Subscription";
import { Config } from "../interfaces/Config";
import { DiscordUtils } from "../utils/discordUtils";
import { RedditService } from "./RedditService";
import { SubscriptionApiService } from "./SubscriptionApiService";
import * as File from '../../config.json';

@Service()
export class SubscriptionService  {

    private cachedTextChannels : TextChannel[] = [];
    private subscriptions : Subscription[] = [];
    private config: Config;

    private bot: Client;

    constructor(private readonly subscriptionApiService: SubscriptionApiService, private readonly redditService: RedditService){
        this.config = File as Config;
    }

    async start() : Promise<any> {

        this.bot = new Client();
        await this.bot.login(this.config.token);
        //Start by getting all the subscriptions
        console.log("Starting subscription service...")
        this.subscriptions = await this.subscriptionApiService.getAll();
        if(!this.subscriptions){
            return "Subscription start failed";
        }
        console.info("Subscriptions service started!");
        //ensure we don't have any subscriptions that don't have any channels.
        this.subscriptions = this.subscriptions.filter(subscription => !subscription.channels || subscription.channels.length !== 0);

        this.subscriptions.forEach((sub) => {
            this.handleSubscription(sub);
        })
    }

    async handleSubscription(subscription: Subscription){
        
        //This subscription no longer has any channels, stop searching for new posts.
        if(subscription.channels.length === 0) {
            this.subscriptions = this.subscriptions.filter(x => x.id != subscription.id);
            return;
        }

        const post = await this.redditService.getNewPost(subscription.subredditName, subscription.lastPostId, subscription.sort);

        if(!post || post === undefined){
            setTimeout(() => {
                this.handleSubscription(subscription);
            }, 10000);
            return;
        }

        subscription.lastPostId = post.postId;
        let channelsThatShouldReceivePost = subscription.channels.filter(x => x.isEnabled);
        if(post.isOver18){
            channelsThatShouldReceivePost = channelsThatShouldReceivePost.filter(x => x.isShowNsfw);
        }

        let textChannels = await this.getTextChannels(channelsThatShouldReceivePost.map(x => x.discordChannelId));
        if(post.isOver18) textChannels = textChannels.filter(channel => channel.nsfw);

        textChannels.forEach((channel) => {
            DiscordUtils.sendPost(post, subscription.subredditName, channel);
        });

        await this.subscriptionApiService.updatePostId(subscription.id, subscription.lastPostId);

        setTimeout(() => {
            this.handleSubscription(subscription);
        }, 10000);
    }

    async getTextChannels(ids: string[]) : Promise<TextChannel[]>{
        //TODO Get from cache if possible
        let channels  : TextChannel[]= [];
        for(const id of ids){
            try{
                const channel = await this.bot.channels.fetch(id) as TextChannel;
                if(channel && channel !== undefined) {
                    this.cachedTextChannels.push(channel as TextChannel);
                    channels.push(channel);
                }
            }
            catch(error){
            }
        }

        return channels;
    }

    addSubscription(subscription: Subscription) {

        //If there already is a subscription for this subreddit we will only need to add the new channel(s) to the subscription.
        const existingSubscription = this.subscriptions.filter(x => x.id == subscription.id)[0];
        if(existingSubscription !== undefined){
            subscription.channels.forEach(channel => {
                existingSubscription.channels.push(channel)
            });
            return;
        }

        //Add it to the cached subscriptions and start the subscription.
        this.subscriptions.push(subscription);
        this.handleSubscription(subscription);
        return;
    }

    removeSubscriptionChannel (subscriptionId: string, channelId: string) {
        const cachedSubscription = this.subscriptions.filter(x => x.id == subscriptionId)[0];

        if(cachedSubscription === undefined) return;

        cachedSubscription.channels = cachedSubscription.channels.filter(x => x.id !== channelId);
    }

    disableGuild(guildId: string) {
        this.subscriptions.forEach(subscription => {
            subscription.channels.forEach(channel => {
                if(channel.guildId === guildId) {

                    channel.isEnabled = false;

                }
            });
        });
    }

    enableGuild(guildId: string) {
        this.subscriptions.forEach(subscription => {
            subscription.channels.forEach(channel => {
                if(channel.guildId === guildId) {

                    channel.isEnabled = true;

                }
            });
        });
    }

    enableNsfw(guildId: string) {
        this.subscriptions.forEach(subscription => {
            subscription.channels.forEach(channel => {
                if(channel.guildId === guildId) {

                    channel.isShowNsfw = false;

                }
            });
        });
    }

    disableNsfw(guildId: string) {
        this.subscriptions.forEach(subscription => {
            subscription.channels.forEach(channel => {
                if(channel.guildId === guildId) {

                    channel.isShowNsfw = true;

                }
            });
        });
    }

    bannedSubreddit(subredditName: string, guildId: string){
        this.subscriptions.filter(x => x.subredditName.toLowerCase() === subredditName.toLowerCase()).forEach(subscription => {
            subscription.channels.forEach(channel => {
                if(channel.guildId === guildId && subscription.subredditName) {
                    channel.isEnabled = false;
                }
            });
        });
    }

    unbannedSubreddit(subredditName: string, guildId: string){
        this.subscriptions.filter(x => x.subredditName.toLowerCase() === subredditName.toLowerCase()).forEach(subscription => {
            subscription.channels.forEach(channel => {
                if(channel.guildId === guildId && subscription.subredditName) {
                    channel.isEnabled = true;
                }
            });
        });
    }

    guildLeft(guildId: string) {
        this.subscriptions.forEach(subscription => {
            subscription.channels = subscription.channels.filter(x => x.guildId !== guildId);
        })
    }
}