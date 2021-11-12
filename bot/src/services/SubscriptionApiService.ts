import { Service } from "typedi";
import { Result } from "../data/Result";
import { Sort } from "../data/Sort";
import { Subscription } from "../data/Subscription";
import { BaseApiService } from "./BaseApiService";

@Service()
export class SubscriptionApiService extends BaseApiService {
    
    cachedSubscriptions: Subscription[] = [];

    constructor(){
        super();
    }

    /**
     * Used to subscribe to a subreddit
     * @param subredditId the subreddit to subscribe to
     * @param sort the sorting method
     * @param channelId the channel id that is subscribing to a subreddit
     * @returns 
     */
    async subscribe(subredditId: string, sort: Sort, channelId: string) : Promise<Result<Subscription>> {
        const data = {subredditId: subredditId, sort: sort, channelId: channelId };

        return this.post<Result<Subscription>>('Subscriptions/Subscribe', JSON.stringify(data)).then((result) => {
            return result.data;
        })
        .catch((error) => {
            return error;
        });
    }

    /**
     * Used to unsubscribe from a subreddit
     * @param subredditId the subreddit id to unsubscribe from
     * @param sort the subscription sorting method
     * @param channelId the subscription channel id
     * @returns 
     */
    async unSubscribe(subredditId: string, sort: Sort, channelId: string) : Promise<Result<string>> {
        const data = {subredditId: subredditId, sort: sort, channelId: channelId };
        return this.post<Result<string>>('Subscriptions/Unsubscribe', JSON.stringify(data)).then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        });
    }

    async getGuildSubscriptions(guildId: string, showNsfw: boolean)  : Promise<Subscription[]> {
        return this.get<Result<Subscription>>(`Subscriptions/GetByGuildId?guildId=${guildId}&showNsfw=${showNsfw}`).then((result) => {
            return result.data.data;
        }).catch(() => {
            return undefined;
        });
    }

    /**
     * Gets all the subscriptions
     * @returns 
     */
    async getAll() : Promise<Subscription[]> {
        return this.get<Result<Subscription>>('Subscriptions/All').then((result) => {
            return result.data.data;
        }).catch(() => {
            return undefined;
        });
    }

    /**
     * Updates a subscription post id
     * @param subscriptionId the subscription id
     * @param lastPostId the new last post id
     * @returns 
     */
    async updatePostId(subscriptionId: string, lastPostId: string) : Promise<Result<any>> {
        const data = {subscriptionId: subscriptionId, lastPostId: lastPostId };
        return this.put<Result<any>>('Subscriptions/UpdatePostId', JSON.stringify(data))
        .then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        });
    }

}