import { Message, MessageEmbed, TextChannel } from "discord.js";
import { User } from "../data/User";
import { Config } from "../interfaces/Config";
import { GuildAdminService } from "../services/GuildAdminService";
import { GuildService } from "../services/GuildService";
import { UserService } from "../services/UserService";
import * as File from '../../config.json';
import Container from "typedi";
import { RedditPost } from "../data/RedditPost";

export abstract class DiscordUtils {

    constructor()
    { }

    static async isAdmin(message: Message, userId?: string) : Promise<boolean>{
        
        const config = File as Config

        //Get the services
        const userService = Container.get(UserService);
        const guildService = Container.get(GuildService);
        const guildAdminService = Container.get(GuildAdminService);

        let user: User;
        if(userId){
            user = await userService.getUser(userId);
        }
        else{
            user = await userService.getUser(message.author.id);
        }

        const guild = await guildService.getGuild(message.guild.id);
        const isAdmin = await guildAdminService.isAdmin(user.id, guild.id);

        if(!isAdmin && message.guild.ownerID !== message.author.id && message.author.id !== config.superUserId){
            return false;
        }
        return true;
    }

    static sendPost(post: RedditPost, subredditName: string, channel: TextChannel, requestedBy?: string) {

        if(post.url.startsWith("https://redgifs") || post.url.startsWith('https://gfycat') || post.url.startsWith("https://i.imgur")){
            channel.send(post.url); 
            return;
        }

        if(post.title.length > 256) {
            post.title = post.title.substring(0, 253);
            post.title += "...";
            
        }
        
        var embededMessage = new MessageEmbed()
            .setTitle(post.title)
            .setImage(post.url)
            .setAuthor(post.author)
            .setURL(post.permalink)
            .setDescription(post.description)
            .setColor("DARK_GOLD")
            .setTimestamp();

        if(requestedBy){
            embededMessage.setFooter(`From: r/${subredditName}, Requested by: ${requestedBy}`)
        }
        else {
            embededMessage.setFooter(`From: r/${subredditName}`)
        }

        channel.send(embededMessage);
    }

}