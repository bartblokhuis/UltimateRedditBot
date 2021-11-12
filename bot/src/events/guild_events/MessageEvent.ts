import { Message } from 'discord.js';
import { readSync } from 'fs';
import Container from 'typedi';
import { Command } from '../../interfaces/Command';
import { RunFunction } from '../../interfaces/Event';
import { GuildService } from '../../services/GuildService';
import { SubredditService } from '../../services/SubredditService';

export const run: RunFunction = async(client, message: Message) => {

    const guildService = Container.get(GuildService);
    var guild = await guildService.getGuild(message.guild.id);

    //Ensure that the message is not from a bot and starts with the configured prefix
    if(message.author.bot || !message.guild || !message.content.toLowerCase().startsWith(guild.setting.prefix)) return;

    //Get the command
    const args: string[] = message.content.slice(guild.setting.prefix.length).trim().split(/ +/g);
    const cmd: string = args.shift();
    let command: Command = client.commands.filter(command => command.name.toLocaleLowerCase() === cmd.toLowerCase() || command.alias.toLowerCase() == cmd.toLowerCase())[0];

    if(!command) {
        //If there is no command check if the user is trying to use the reddit post command
        const subredditSerivce = Container.get(SubredditService);
        const subreddit = subredditSerivce.getByName(cmd);

        if(!subreddit) return;

        args.unshift(cmd);
        command = client.commands.filter(command => command.name == 'reddit')[0];
    }

    if(!guild.setting.isEnabled && command.name !== 'enabled') return;
    
    //Execute the command
    command.run(client, message, args).catch((reaason: any) => {
        message.channel.send(client.embed({description: `An error happened: ${readSync}`}, message));
    });

}

export const name: string = 'message';