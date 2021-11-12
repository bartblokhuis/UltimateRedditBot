import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildService } from '../../services/GuildService';
import { SubscriptionService } from '../../services/SubscriptionService';
import { DiscordUtils } from '../../utils/discordUtils';

export const run: RunFunction = async(client, message, args) => {

    if(!args || args.length === 0){
        getAllowNsfw();
        return;
    }
    else {
        updateAllowNsfw();
    }

    async function getAllowNsfw() {
        const guildService = Container.get(GuildService)
        const guild = await guildService.getGuild(message.guild.id);

        if(guild.setting.allowNsfw){
            message.channel.send("Nsfw posts are currently allowed.");
        }
        else{
            message.channel.send("Nsfw posts are currently not allowed");
        }
    }

    async function updateAllowNsfw(){
        const isNsfwString = args[0].toString().toLowerCase();
    
        if(isNsfwString !== 'false' && isNsfwString !== 'true' && isNsfwString !== 'off' && isNsfwString !== 'on'){
            message.channel.send('Please enter a true or false');
            return;
        }
    
        const guildService = Container.get(GuildService)
        const guild = await guildService.getGuild(message.guild.id);

        if(!await DiscordUtils.isAdmin(message)){
            message.channel.send("Only mods can change the nsfw setting");
            return;
        }
    
        if(guild.setting.allowNsfw.toString().toLocaleLowerCase() === isNsfwString){
            message.channel.send(`Nsfw is already set to ${isNsfwString}`)
            return;
        }

        const subscriptionService = Container.get(SubscriptionService);
        if(isNsfwString === 'true' || isNsfwString === 'on'){
            guild.setting.allowNsfw = true;
            
        }
        else{
            guild.setting.allowNsfw = false;
        }

        await guildService.update(guild.id, guild.setting.prefix, guild.setting.allowNsfw, guild.setting.isEnabled, guild.setting.sort);
        
        if(guild.setting.allowNsfw){
            subscriptionService.enableNsfw(guild.id)
        }
        else {
            subscriptionService.disableNsfw(guild.id)
        }

        message.channel.send(`Nsfw is now: ${isNsfwString}`);
    }
};

export const name: string = 'allow-nsfw';
export const alias: string = 'allow-nsfw';