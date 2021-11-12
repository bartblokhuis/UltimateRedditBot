import { RunFunction } from '../../interfaces/Command';
import Container from 'typedi';
import { GuildService } from '../../services/GuildService';
import { PlaylistService } from '../../services/PlaylistService';
import { SubredditService } from '../../services/SubredditService';
import { Message, TextChannel } from 'discord.js';
import { DiscordUtils } from '../../utils/discordUtils';

export const run: RunFunction = async(client, message, args) => {
   
    if(!args || args.length === 0){
        message.channel.send('Please provide a name');
        return;
    }

    if(args.length > 1){
        addSubredditToPlaylist(message, args);
        return;
    }
    
    addPlaylist(message, args);

    async function addPlaylist(message, args) {

        if(!await DiscordUtils.isAdmin(message)){
            message.channel.send("Only mods can make new playlists");
            return;
        }

        const name = getPlaylistName(args);
        if(name.length > 12){
            message.channel.send('Please choose a name with less than 12 charachters.');
            return;
        }

        const guildService = Container.get(GuildService);
        const guild = await guildService.getGuild(message.guild.id);

        const playlistService = Container.get(PlaylistService);
        const addPlaylistResult = await playlistService.add(name, guild.id);
        if(!addPlaylistResult.succeeded){
            message.channel.send(addPlaylistResult.messages[0]);
            return;
        }
        else{
            message.channel.send(`Added playlist: ${name}`);
        }
    }

    async function addSubredditToPlaylist(message : Message, args){
        
        if(!await DiscordUtils.isAdmin(message)){
            message.channel.send("Only mods can add subreddits to a playlist");
            return;
        }

        const guildService = Container.get(GuildService);
        const guild = await guildService.getGuild(message.guild.id);

        const name = getPlaylistName(args);
        const playlistService = Container.get(PlaylistService);
        const playlist = await playlistService.getByName(name, guild.id);
        if(!playlist){
            message.channel.send("Playlist does not exist.");
        }

        const subredditService = Container.get(SubredditService);
        const subreddit = await subredditService.getByName(args[1]);

        if(!subreddit){
            message.channel.send("Subreddit doesn't exist");
            return;
        }

        const result = await playlistService.addSubreddit(playlist.id, subreddit.id);
        message.channel.send(result.messages[0]);
    }


    function getPlaylistName(args: string[]) {
        return capitalize(args[0]);
    }

    function capitalize(str: string): string{
        const lower = str.toLocaleLowerCase();
        return str.charAt(0).toUpperCase() + lower.slice(1);
    }

};


export const name: string = 'playlist-add';
export const alias: string = 'p-add';