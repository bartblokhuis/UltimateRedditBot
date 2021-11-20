import { Service } from 'typedi';
import { Result } from '../data/Result';
import { Subreddit } from '../data/Subreddit';
import { APIService } from './APIService';
import { BaseApiService } from './BaseApiService';
import { CacheService } from './CacheService';
import { RedditService } from './RedditService';

@Service()
export class SubredditService extends BaseApiService {

    private static cacheStart: string = "subreddit.";
    private static cacheLifetimeInMiliseconds: number = 3600;
    cachedSubreddits: Subreddit[] = [];

    constructor(private readonly redditService: RedditService, private readonly api: APIService, private readonly cacheService: CacheService){
        super();
    }

    /**
     * 
     * @param subredditName the subreddit name
     * @returns 
     */
    async getByName(subredditName) : Promise<Subreddit> {
        
        //Try gettin from the cache.
        let subreddit = this.cacheService.get<Subreddit>(SubredditService.cacheStart + subredditName.toLowerCase())
        if(subreddit) return subreddit; //Loaded from cache.

        //Try getting from the API
        subreddit = await this.getFromApi(subredditName);

        if(subreddit){
            subreddit.name = subreddit.name;
            this.cachedSubreddits.push(subreddit); //Add to cache.
            return subreddit;
        }

        //Try getting the subreddit from reddit.
        subreddit = await this.redditService.getSubredditByName(subredditName);

        if(!subreddit) return null; //Subreddit not found.

        //Save the subreddit
        subreddit = await this.saveSubreddit(subreddit);

        return subreddit;
    }

    /**
     * Adds an subreddit to the api, this is faster than to keep getting it from reddit
     * @param subreddit 
     * @returns 
     */
    async saveSubreddit(subreddit: Subreddit) : Promise<Subreddit> {
        subreddit.name = subreddit.name.toLocaleLowerCase();
        subreddit = await this.add(subreddit.name, subreddit.isNsfw);

        //Push the subreddit to the cache
        this.cacheService.put(SubredditService.cacheStart + subreddit.name.toLowerCase(), subreddit, SubredditService.cacheLifetimeInMiliseconds)
        return subreddit;
    }

    /**
     * Gets an subreddit from the API
     * @param subredditName the subreddit name
     * @returns 
     */
    private async getFromApi(subredditName: string): Promise<Subreddit>{

        return this.get<Result<Subreddit>>(`Subreddit/Get?subredditName=${subredditName}`).then((result) => {
                if(result.data.succeeded) return result.data.data;
                return undefined;
            })
            .catch(() => {
                return undefined
            });
    }

    /**
     * Creates a new subreddit in the API
     * @param name the subreddit name
     * @param isNsfw is the subreddit nsfw
     * @returns 
     */
    async add(name: string, isNsfw: boolean): Promise<Subreddit>{
        const subreddit = JSON.stringify({ name: name, isNsfw: isNsfw });

        return this.post<Result<Subreddit>>('Subreddit/Add', subreddit).then((result) => {
                return result.data.data;
            }).catch(() => {
                return undefined;
            });

    }

    /**
     * Checks if the subreddit is banned in the guild
     * @param guildId the guild id
     * @param subredditId the subreddit id
     * @returns 
     */
    async isSubredditBanned( guildId: string, subredditId: string) : Promise<boolean> {

        return this.get<Result<boolean>>(`BannedSubreddits/IsBanned?guildId=${guildId}&subredditId=${subredditId}`)
            .then((result) => {
                return result.data.data;
            })
            .catch((error) => {
                console.error(error);
                return false;
            });
    }

    /**
     * Ban a subreddit in a guild
     * @param subredditId the subreddit id
     * @param guildId the guild id
     * @returns 
     */
    async banSubreddit(subredditId: string, guildId: string): Promise<any> {
        var data = { subredditId: subredditId, guildId: guildId };
        return this.post('BannedSubreddits/Ban', JSON.stringify(data));
    }

    /**
     * Used to unban a subreddit
     * @param subredditId the banned subreddit id
     * @param guildId the guild id where the subreddit is banned
     * @returns 
     */
    async unBanSubreddit(subredditId: string, guildId: string): Promise<Result<any>> {
        var data = { subredditId: subredditId, guildId: guildId };
        return this.post<Result<any>>('BannedSubreddits/Unban', JSON.stringify(data)).then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        });
    }
}