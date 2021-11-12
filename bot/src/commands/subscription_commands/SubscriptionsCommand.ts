import { channel } from 'diagnostic_channel';
import { EmbedFieldData, MessageEmbed, TextChannel } from 'discord.js';
import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';
import { SubscriptionApiService } from '../../services/SubscriptionApiService';

export const run: RunFunction = async(client, message, args) => {
    
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    const subscriptionApiService = Container.get(SubscriptionApiService);
    var showNsfw = guild.setting.allowNsfw && (message.channel as TextChannel).nsfw;

    const subscriptions = await subscriptionApiService.getGuildSubscriptions(guild.id, showNsfw);
 
    let fields: EmbedFieldData[] = [];

    subscriptions.forEach(subscription => {

        var valueBuilder = "";
        subscription.channels.forEach(channel => {
            valueBuilder += `<#${channel.discordChannelId}> \n`
        });

        fields.push({ name: subscription.subredditName, value: valueBuilder, inline: false} )
    });

    var embededMessage = new MessageEmbed()
            .setTitle("Subscriptions")
            .addFields(fields)
            .setColor("DARK_GOLD")
            .setTimestamp();

    message.channel.send(embededMessage);
};

export const name: string = 'subscriptions';
export const alias: string = 'subs';