import { Service } from "typedi";
import { PostHistory } from "../data/PostHistory";
import { Result } from "../data/Result";
import { Sort } from "../data/Sort";
import { BaseApiService } from "./BaseApiService";

@Service()
export class PostHistoryService extends BaseApiService {

    cachedPostHistory: PostHistory[] = [];

    constructor(){
        super();
    }

    /**
     * Used to get the last post id for a subreddit
     * @param guildId The guild id
     * @param subredditId The subreddit id
     * @param sort The sorting method
     * @returns 
     */
    async getHistory(guildId: string, subredditId: string, sort: Sort) : Promise<PostHistory> {

        //Try getting from cache
        let postHistory = this.getFromCache(guildId, subredditId, sort);
        if(postHistory) return postHistory; //Found in cache.

        return this.get<Result<PostHistory>>( `PostHistories?guildId=${guildId}&subredditId=${subredditId}&sort=${sort}`).then((result) => {
                if(!result.data.succeeded) return undefined;

                //add to the cache
                this.cachedPostHistory.push(result.data.data);
                return result.data.data;
            }).catch(() => {
                return undefined;
            });
    };

    /**
     * Update posthistories
     * @param postHistories The posthistories that need to get updated
     * @returns 
     */
    async save(postHistories: PostHistory[]) : Promise<any> {

        var jsonPostHistoriesBuilder = {
            "postHistories": postHistories
        };

        //Update the cache
        postHistories.forEach(postHistory => {
            let cachedItem = this.getFromCache(postHistory.guildId, postHistory.subredditId, postHistory.sort);
            if(cachedItem){
                cachedItem.lastPostId = postHistory.lastPostId;
            }
            else{
                this.cachedPostHistory.push(postHistory);
            }
        });

        return this.post('PostHistories', jsonPostHistoriesBuilder).then((result) => {
            return result.data;
        });
    }

    /**
     * Gets post history from the cache
     * @param guildId the guild id
     * @param subredditId the subreddit id
     * @param sort the sorting method
     * @returns 
     */
    private getFromCache(guildId: string, subredditId: string, sort: Sort) : PostHistory {
        return this.cachedPostHistory.filter(postHistory => postHistory.guildId == guildId && postHistory.subredditId == subredditId && postHistory.sort == sort)[0];
    }

}