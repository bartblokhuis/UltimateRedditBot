import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildAdminService } from '../../services/GuildAdminService';
import { GuildService } from '../../services/GuildService';
import { UserService } from '../../services/UserService';

export const run: RunFunction = async(client, message, args) => {
    if(!args || args.length === 0){
        message.channel.send("Please mention the user you want to mod.");
        return;
    }
  
    if(message.guild.ownerID !== message.author.id){
        message.channel.send("Only server owners can make other people mods.");
        return;
    }

    const user = message.mentions.members.first();
    if(!user){
        message.channel.send("Please mention the user you want to mod.");
        return;
    }

    if(message.guild.ownerID === user.id){
        message.channel.send("This user will always have mod permissions");
        return;
    }

    const guildService = Container.get(GuildService);
    const userService = Container.get(UserService);
    const guildAdminService = Container.get(GuildAdminService);

    const guild = await guildService.getGuild(message.guild.id);
    const apiUser = await userService.getUser(user.id);

    guildAdminService.addAdmin(apiUser.id, guild.id)
        .then((result) => {
            if(!result.data.succeeded && result.data.messages[0] === 'User is already an admin') {
                message.channel.send(`${user.toString()}  is already a mod`);
                return;
            }

            message.channel.send(`${user.toString()} is now a mod`);
        });
};

export const name: string = 'mod';
export const alias: string = 'mod';