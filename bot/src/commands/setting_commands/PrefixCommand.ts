import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';
import { DiscordUtils } from '../../utils/discordUtils';

export const run: RunFunction = async(client, message, args) => {
    if(!args || args.length === 0){
        message.channel.send('Please provide a new prefix');
        return;
    }

    const prefix = args[0].toString();
    if(prefix.length > 4){
        message.channel.send("The prefix can't have more than 4 charachters");
        return
    }
    
    const guildService = Container.get(GuildService)
    const guild = await guildService.getGuild(message.guild.id);

    if(!await DiscordUtils.isAdmin(message)){
        message.channel.send("Only mods can change the prefix");
        return;
    }

    if(guild.setting.prefix === prefix){
        message.channel.send(`The prefix already is ${prefix}`)
        return;
    }
    guild.setting.prefix = prefix;

    await guildService.update(guild.id, guild.setting.prefix, guild.setting.allowNsfw, guild.setting.isEnabled, guild.setting.sort);

    message.channel.send(`Prefix is now: ${prefix}`);


};

export const name: string = 'prefix';
export const alias: string = 'prefix';