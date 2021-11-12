import { MessageEmbed, TextChannel } from 'discord.js';
import { Service } from 'typedi';
import { Guild } from '../data/Guild';
import { GuildQueueItem } from '../data/GuildQueueItem';
import { PostHistory } from '../data/PostHistory';
import { QueueItem } from '../data/QueueItem';
import { Sort } from '../data/Sort';
import { Subreddit } from '../data/Subreddit';
import { DiscordUtils } from '../utils/discordUtils';
import { PostHistoryService } from './PostHistoryService';
import { RedditService } from './RedditService';

@Service()
export class QueueService{

    public guildQueueItems: GuildQueueItem[] = [];

    constructor (private readonly redditService: RedditService, private readonly postHistoryService : PostHistoryService){
        this.processQueue();
    }

    async addToQueue(subreddit: Subreddit, channel: TextChannel, amountOfPosts: number, guild: Guild, redditSort: Sort, requestedBy: string): Promise<void> {
        let guildQueueItem = this.guildQueueItems.filter(guildQueue => guildQueue.guild.id == guild.id)[0];
        if(!guildQueueItem){
            this.guildQueueItems.push({ guild: guild, isRunning: false, queueItems: [] });
            guildQueueItem = this.guildQueueItems.filter(guildQueue => guildQueue.guild.id == guild.id)[0];
        }

        let queueItem = guildQueueItem.queueItems.filter((item) => item.subreddit.id == subreddit.id)[0];
       
        if(!queueItem){
            //Queue item doesn't exist yet, add a new one.
            var lastPost = await this.postHistoryService.getHistory(guild.id, subreddit.id, redditSort);
            let lastUsedPostName: string = '';
            if(lastPost) lastUsedPostName = lastPost.lastPostId;

            queueItem = { amountOfPosts: amountOfPosts, channel: channel, subreddit: subreddit, lastUsedPostName: lastUsedPostName, failedGetAttemppts: 0, sort: redditSort, guildId: guild.id, requestedBy: requestedBy};
            guildQueueItem.queueItems.push(queueItem);
            return;
        }
        
        if(queueItem.channel.id !== channel.id){
            channel.send(`The subreddit ${subreddit.name} is already in the queue in the #${queueItem.channel.name} channel :)`);
        }
        queueItem.amountOfPosts += amountOfPosts;
        
    }

    async processQueue(): Promise<any> {
        if(this.guildQueueItems.length === 0) {
            await new Promise(res => setTimeout(res, 1000));
            this.processQueue();
            return;
        }

        this.guildQueueItems.filter(guildQueueItem => !guildQueueItem.isRunning).forEach(guildQueueItem => {

            guildQueueItem.isRunning = true

            this.proccessGuildQueueItem(guildQueueItem.queueItems).then(async () => {
                guildQueueItem.isRunning = false;
                
                const completedQueueItems = guildQueueItem.queueItems.filter(queueItem => queueItem.amountOfPosts == 0);
                const postHistories: PostHistory[] = [];
                completedQueueItems.forEach(queueItem => postHistories.push( { guildId: queueItem.guildId,  lastPostId: queueItem.lastUsedPostName, sort: queueItem.sort, subredditId: queueItem.subreddit.id }));

                if(postHistories.length > 0){
                    await this.postHistoryService.save(postHistories);
                }
                
                guildQueueItem.queueItems = guildQueueItem.queueItems.filter(queueItem => queueItem.amountOfPosts > 0);
            }).catch(() => {
                guildQueueItem.isRunning = false;
                guildQueueItem.queueItems = guildQueueItem.queueItems.filter(queueItem => queueItem.amountOfPosts > 0);
            })
        });

        await new Promise(res => setTimeout(res, 1000));
        this.processQueue();
    }

    async proccessGuildQueueItem(queueItems: QueueItem[]) {

       await Promise.all(queueItems.map(async (queueItem) => {
            await this.processQueueItem(queueItem);
            return;
       }));

        await new Promise(res => setTimeout(res, 1000));
    }

    async processQueueItem(queueItem: QueueItem) : Promise<any> {
        
        //Get the posts.
        const posts = await this.redditService.getPosts(queueItem.subreddit.name, queueItem.lastUsedPostName, queueItem.sort);
        if(posts.length === 0){
            //If we failed to get any posts the previous post must have been deleted or we have reachted the last 
            //post in the subreddit. either way we want to reset the last saved post so we can continue getting posts.
            queueItem.lastUsedPostName = '';
            queueItem.failedGetAttemppts++;
            return;
        }

        //Filter out any posts are stickied or dont have an video / image.
        var postsWithImage = posts.filter((post) => post.stickied == false && post.url && post.postId != queueItem.lastUsedPostName &&
            (post.url.endsWith(".jpg") || post.url.endsWith(".jpeg") || post.url.endsWith(".png") || post.url.endsWith(".gif") || post.url.startsWith("https://gfycat") || post.url.startsWith("https://redgifs") || post.url.endsWith(".mp4") ));

        if(postsWithImage.length === 0) {
            queueItem.lastUsedPostName = posts[posts.length-1].postId;
            queueItem.failedGetAttemppts++;
            return;
        }

        const delay = ms => new Promise(resolve => setTimeout(resolve, ms));

        await (async function loop() {
            for (let i = 0; i < queueItem.amountOfPosts; i++) {

                const post = postsWithImage[i];
                if(!post){
                    queueItem.amountOfPosts = queueItem.amountOfPosts - i;
                    queueItem.lastUsedPostName = postsWithImage[i - 1].postId;
                    return;
                }

                DiscordUtils.sendPost(post, queueItem.subreddit.name, queueItem.channel, queueItem.requestedBy);
                await delay(800);
            }
        })().then(() => {
            const lastPost = postsWithImage[queueItem.amountOfPosts - 1];

            queueItem.amountOfPosts = 0;
            queueItem.lastUsedPostName = lastPost.postId;
        });
    }

}
