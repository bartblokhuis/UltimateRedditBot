import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';
import { SubscriptionService } from '../../services/SubscriptionService';
import { DiscordUtils } from '../../utils/discordUtils';

export const run: RunFunction = async(client, message, args) => {

    if(!args || args.length === 0){
        message.channel.send('Please provide a new prefix');
        return;
    }

    const isEnabledString = args[0].toString().toLowerCase();
    
    if(isEnabledString !== 'false' && isEnabledString !== 'true'){

        message.channel.send('Please enter a true or false');
        return;
    }
    
    const guildService = Container.get(GuildService)
    const guild = await guildService.getGuild(message.guild.id);

    if(!await DiscordUtils.isAdmin(message)){
        message.channel.send("Only mods can enable or disable the bot");
        return;
    }

    if(guild.setting.isEnabled.toString().toLocaleLowerCase() === isEnabledString){

        if(guild.setting.isEnabled) message.channel.send(`The bot is already enabled`)
        else message.channel.send(`The bot is already disabled`)
        
        return;
    }

    if(isEnabledString === 'true'){
        guild.setting.isEnabled = true;
    }
    else{
        guild.setting.isEnabled = false;
    }

    await guildService.update(guild.id, guild.setting.prefix, guild.setting.allowNsfw, guild.setting.isEnabled, guild.setting.sort);

    const subscriptionService = Container.get(SubscriptionService);
    if(guild.setting.isEnabled) {
        subscriptionService.enableGuild(guild.id);
        message.channel.send('The bot is now enabled');
    }
    else{
        subscriptionService.disableGuild(guild.id);
        message.channel.send("The bot is now disabled");
    }


};

export const name: string = 'enabled';
export const alias: string = 'enabled';