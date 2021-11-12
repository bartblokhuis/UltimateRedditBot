import { Guild } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Event';
import { GuildService } from '../../services/GuildService';
import { SubscriptionService } from '../../services/SubscriptionService';

export const run: RunFunction = async(client, guild: Guild) => {
    const guildService = Container.get(GuildService);

    const apiGuild = await guildService.getGuild(guild.id);
    if(apiGuild === undefined){
        return;
    }

    await guildService.leave(apiGuild.id);

    const subscriptionService = Container.get(SubscriptionService);
    subscriptionService.guildLeft(apiGuild.id);
}

export const name: string = 'guildDelete';