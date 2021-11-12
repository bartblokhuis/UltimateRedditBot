import { Service } from 'typedi';
import { BaseApiService } from './BaseApiService';
import { Guild } from '../data/Guild';
import { Result } from '../data/Result';
import { Sort } from '../data/Sort';
import { CacheService } from './CacheService';

export class Test {
}

@Service()
export class GuildService extends BaseApiService {

    private static cacheStart: string = "guild.";
    private static guildCacheLifetimeInMiliseconds: number = 3600;

    constructor(private readonly cacheService: CacheService) {
        super()
    }

    /**
     * Used to get a guild from the api
     * @param discordId the discord guild id
     * @returns 
     */
    async getGuild(discordId: string) : Promise<Guild> {

        let guild = this.cacheService.get<Guild>(GuildService.cacheStart + discordId)
        if(guild) return guild;

        return this.get<Result<Guild>>(`Guild/Get?discordGuildId=${discordId}`).then((result) => {
            this.cacheService.put(GuildService.cacheStart + discordId, result.data.data, GuildService.guildCacheLifetimeInMiliseconds)
                return result.data.data;
            }).catch(() => {
                return undefined;
            });
    }

    /**
     * Used to update a guild or the guild settings
     * @param guildId the guild id
     * @param prefix the commands prefix
     * @param allowNsfw wether or not nsfw is allowed
     * @param isEnabled used to disable the bot entirely
     * @param sort the default sorting method
     * @returns 
     */
    async update(guildId: string, prefix: string, allowNsfw: boolean, isEnabled: boolean, sort: Sort) : Promise<Guild> {
        
        var data = { guildId: guildId, prefix: prefix, allowNsfw: allowNsfw, isEnabled: isEnabled, sort: sort };
        return this.put<Result<Guild>>(`Guild`, JSON.stringify(data)).then((result) => {
            this.cacheService.put(GuildService.cacheStart + result.data.data.discordGuildId, result.data.data, GuildService.guildCacheLifetimeInMiliseconds);
            return result.data.data;
        }).catch(() => {
            return undefined;
        });
    }

    /**
     * 
     * @param guildId 
     */
     async leave(guildId: string): Promise<boolean> {
        const data = { guildId: guildId };
        const request = await this.delete<Result<any>>(`Guild/Leave`, JSON.stringify(data))
        return request.data.succeeded;
    }
}