import { RunFunction } from '../../interfaces/Command';
import Container from 'typedi';
import { GuildService } from '../../services/GuildService';
import { PlaylistService } from '../../services/PlaylistService';
import { SubredditService } from '../../services/SubredditService';
import { Message } from 'discord.js';
import { DiscordUtils } from '../../utils/discordUtils';

export const run: RunFunction = async(client, message, args) => {
   
    if(!args || args.length === 0){
        message.channel.send('Please provide a name');
        return;
    }

    if(args.length > 1){
        removeSubredditFromPlaylist(message, args);
        return;
    }
    
    removePlaylist(message, args);

    async function removePlaylist(message, args) {

        if(!await DiscordUtils.isAdmin(message)){
            message.channel.send("Only mods remove playlists");
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
        const playlist = await playlistService.getByName(name, guild.id);

        if(!playlist) {
            message.channel.send("Playlist doesn't exist");
            return;
        }
        if(playlist.isGlobal){
            message.channel.send("You can't remove a global playlist");
            return;
        }

        const removePlaylistResult = await playlistService.remove(playlist.id);
        if(!removePlaylistResult.succeeded){
            message.channel.send(removePlaylistResult.messages[0]);
            return;
        }
        else{
            message.channel.send(`Removed playlist: ${name}`);
        }
    }

    async function removeSubredditFromPlaylist(message : Message, args){
        
        if(!await DiscordUtils.isAdmin(message)){
            message.channel.send("Only mods can remove a subreddit from a playlist");
            return;
        }

        const guildService = Container.get(GuildService);
        const guild = await guildService.getGuild(message.guild.id);

        const name = getPlaylistName(args);
        const playlistService = Container.get(PlaylistService);

        const playlist = await playlistService.getByName(name, guild.id);
        if(!playlist){
            message.channel.send("Playlist does not exist.");
            return;
        }
        if(playlist.isGlobal){
            message.channel.send("You can't remove subreddits from a global playlist");
            return;
        }


        const subredditService = Container.get(SubredditService);
        const subreddit = await subredditService.getByName(args[1]);

        if(!subreddit){
            message.channel.send("Subreddit doesn't exist");
            return;
        }

        const result = await playlistService.removePlaylistSubreddit(playlist.id, subreddit.id);
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

export const name: string = 'playlist-remove';
export const alias: string = 'p-remove';