import Container from 'typedi';
import { RunFunction } from '../../interfaces/Command';
import { GuildAdminService } from '../../services/GuildAdminService';
import { GuildService } from '../../services/GuildService';
import { UserService } from '../../services/UserService';

export const run: RunFunction = async(client, message, args) => {
    if(!args || args.length === 0){
        message.reply("Please mention the user you want to unmod.");
        return;
    }

    if(message.guild.ownerID !== message.author.id){
        message.reply("Only server owners can unmod people.");
        return;
    }

    const user = message.mentions.members.first();
    if(!user){
        message.reply("Please mention the user you want to unmod.");
        return;
    }

    const guildService = Container.get(GuildService);
    const userService = Container.get(UserService);
    const guildAdminService = Container.get(GuildAdminService);

    const guild = await guildService.getGuild(message.guild.id);
    const apiUser = await userService.getUser(user.id);

    guildAdminService.removeAdmin(apiUser.id, guild.id)
        .then((result) => {
            if(!result.data.succeeded && result.data.messages[0] === 'User is not a mod') {
                message.channel.send(`${user.toString()} is not a mod`);
                return;
            }

            message.channel.send(`${user.toString()} is no longer a mod`);
        })
        .catch((error) => {
            console.log(error);
        });
}
export const name: string = 'unmod';
export const alias: string = 'unmod';