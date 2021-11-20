import { RunFunction } from '../../interfaces/Command';
import Container from 'typedi';
import { GuildService } from '../../services/GuildService';
import { PlaylistService } from '../../services/PlaylistService';
import { EmbedFieldData, Message, MessageEmbed, TextChannel } from 'discord.js';

export const run: RunFunction = async(client, message, args) => {

    const playListService = Container.get(PlaylistService);
    const guildService = Container.get(GuildService);
    const guild = await guildService.getGuild(message.guild.id);

    if(args.length > 0){
        getPlaylistInfo(message, args)
        return;
    }

    getPlaylistsInfo(message);

    async function getPlaylistInfo(message: Message, args: string[]){
        const name = capitalize(args[0]);

        const playlist = await playListService.getByName(name, guild.id);

        let fields: EmbedFieldData[] = [];

        const nsfwSubreddits = playlist.subreddits.filter(x => x.isNsfw);
        const nonNsfwSubreddits = playlist.subreddits.filter(x => !x.isNsfw);
        
        if(nonNsfwSubreddits.length > 1){
            let builder = ""
            nonNsfwSubreddits.forEach(element => {
                builder += element.name + "\n";
            });
            fields.push({name: "Subreddits", value: builder});
        }

        if(nsfwSubreddits.length > 0 && (message.channel as TextChannel).nsfw && guild.setting.allowNsfw){
            let builder = ""
            nsfwSubreddits.forEach(element => {
                builder += element.name + "\n";
            });

            fields.push({name: "NSFW Subreddits", value: builder});
        }

        var embededMessage = new MessageEmbed()
                    .setTitle(playlist.name)
                    .addFields(fields)
                    .setTimestamp();

        message.channel.send(embededMessage);
    }

    async function getPlaylistsInfo(message: Message){
        const playlists = await playListService.getByGuildId(guild.id);
        console.log(playlists);
        if(!playlists){
            message.channel.send("This server has no playlists");
            return;
        }
        let fields: EmbedFieldData[] = [];

        const globalPlaylists = playlists.data.filter(x => x.isGlobal);
        const guildPlaylists = playlists.data.filter(x => !x.isGlobal);
        
        if(globalPlaylists.length > 1){
            let builder = ""

            globalPlaylists.forEach(element => {
                builder += element.name + "\n";
            });
            fields.push({name: "Global playlists", value: builder});
        }

        if(guildPlaylists.length > 0){
            let builder = ""
            guildPlaylists.forEach(element => {
                builder += element.name + "\n";
            });

            fields.push({name: "Guild playlists", value: builder});
        }

        var embededMessage = new MessageEmbed()
                    .setTitle("Playlists")
                    .addFields(fields)
                    .setTimestamp();

        message.channel.send(embededMessage);
    }

    function capitalize(str: string): string{
        const lower = str.toLocaleLowerCase();
        return str.charAt(0).toUpperCase() + lower.slice(1);
    }
};


export const name: string = 'playlist-info';
export const alias: string = 'p-info';