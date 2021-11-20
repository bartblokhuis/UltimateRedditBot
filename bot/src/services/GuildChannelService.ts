import { Service } from "typedi";
import { GuildChannel } from "../data/GuildChannel";
import { Result } from "../data/Result";
import { BaseApiService } from "./BaseApiService";

@Service()
export class GuildChannelService extends BaseApiService {

    cachedChannels: GuildChannel[] = [];

    /**
     * Used to get a guild's channel.
     * @param guildId channels guild id
     * @param discordChannelId the discord channel id
     * @returns 
     */
    async getChannel(guildId: string, discordChannelId: string): Promise<GuildChannel>{

        const cachedGuildChannel = this.cachedChannels.filter(channel => channel.discordChannelId === discordChannelId && channel.guildId === guildId)[0];
        if(cachedGuildChannel){
            return cachedGuildChannel;
        }

        return this.get<Result<GuildChannel>>(`GuildChannels?guildId=${guildId}&discordChannelId=${discordChannelId}`).then((result) => {
            this.cachedChannels.push(result.data.data);
                return result.data.data;
            }).catch(() => {
                return undefined;
            })
    }
}