import Container from 'typedi';
import { Sort } from '../../data/Sort';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';
import { DiscordUtils } from '../../utils/discordUtils';

export const run: RunFunction = async(client, message, args) => {
    if(!args || args.length === 0){
        message.channel.send('Please provide a new prefix');
        return;
    }

    const guildService = Container.get(GuildService)
    const guild = await guildService.getGuild(message.guild.id);

    if(!await DiscordUtils.isAdmin(message)){
        message.channel.send("Only mods can change the sort setting");
        return;
    }

    let mayBeSort: Sort | undefined = (<any>Sort)[args[0].toLowerCase()];
    if (mayBeSort === undefined){
        message.channel.send(`Please enter a real sorting option, use '${guild.setting.prefix}help' for more info.`)
        return;
    } 

    if(guild.setting.sort === mayBeSort){
        message.channel.send(`The default sort method is already set to ${guild.setting.sort}`)
        return;
    }

    guild.setting.sort = mayBeSort;

    await guildService.update(guild.id, guild.setting.prefix, guild.setting.allowNsfw, guild.setting.isEnabled, guild.setting.sort);

    message.channel.send(`Default sorting method is now: ${guild.setting.sort.toString()}`);


};

export const name: string = 'sort';
export const alias: string = 'sort';