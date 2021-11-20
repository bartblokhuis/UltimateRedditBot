import { Service } from "typedi";
import { Playlist } from "../data/Playlist";
import { Result } from "../data/Result";
import { BaseApiService } from "./BaseApiService";

@Service()
export class PlaylistService extends BaseApiService {

    cachedPlaylists: Playlist[] = [];

    constructor(){
        super();
    }

    /**
     * Gets the guild's and global playlists
     * @param guildId the guild id
     * @returns 
     */
    async getByGuildId(guildId: string) : Promise<Result<Playlist[]>>{
        return this.get<Result<Playlist[]>>(`Playlists/GetByGuildId?guildId=${guildId}`)
        .then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        })
    }

    /**
     * Get a playlist by it's name and guild id
     * @param name the playlist's name
     * @param guildId the playlist's guild
     * @returns 
     */
    async getByName(name: string, guildId: string) : Promise<Playlist> {

        const cachedPlaylist = this.cachedPlaylists.filter(playlist => playlist.name.toLowerCase() === name.toLowerCase() && (playlist.isGlobal || playlist.guildId === guildId))[0];
        if(cachedPlaylist){
            return cachedPlaylist;
        }

        return this.get<Result<Playlist>>(`Playlists/GetByName?name=${name}&guildId=${guildId}`)
            .then((result) => {

                this.cachedPlaylists.push(result.data.data);
                if(!result.data.succeeded) return undefined;
                
                return result.data.data;
            }).catch(() => {
                return undefined;
            })
    }

    /**
     * Creates a new playlist
     * @param name the playlist's name
     * @param guildId the playlist's guild id
     * @returns 
     */
    async add(name: string, guildId: string): Promise<Result<any>>{
        const data = { name: name, guildId: guildId, isGlobal: false  }
        return this.post<Result<any>>('Playlists/Add', JSON.stringify(data)).then((result) => {
            if(result.data.succeeded){
                this.cachedPlaylists.push(result.data.data);
            }
            return result.data
        })
        .catch(() => {
            return undefined;
        })
    }

    /**
     * Adds a subreddit to a playlist
     * @param playlistId the playlist id
     * @param subredditId the subreddit id
     * @returns 
     */
    async addSubreddit(playlistId: string, subredditId: string) : Promise<Result<any>> {
        const data = { playlistId: playlistId, subredditId: subredditId };

        return this.post<Result<any>>('Playlists/AddSubreddit', JSON.stringify(data)).then((result) => {
            if(result.data.succeeded){
                let cachedPlaylist = this.cachedPlaylists.filter(playlist => playlist.id == playlistId)[0];
                if(cachedPlaylist === undefined) this.cachedPlaylists.push(result.data.data);
                
                var index = this.cachedPlaylists.indexOf(cachedPlaylist);
                this.cachedPlaylists[index] = result.data.data;
            }
            return result.data
        })
        .catch(() => {
            return undefined;
        })
    }

    /**
     * Removes an playlist
     * @param playlistId the playlist id that needs to be removed.
     * @returns 
     */
    async remove(playlistId: string) : Promise<Result<any>>{
        const data = { playlistId: playlistId };

        this.cachedPlaylists = this.cachedPlaylists.filter(playlist => playlist.id !== playlistId);

        return this.delete<Result<any>>('Playlists/Remove', data)
        .then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        })
    }

    /**
     * Removes a subreddit from a playlist
     * @param playlistId the playlist id
     * @param subredditId the subreddit id that needs to be removed
     * @returns 
     */
    async removePlaylistSubreddit(playlistId: string, subredditId) : Promise<Result<any>> {
        const data = { playlistId: playlistId, subredditId: subredditId };

        return this.delete<Result<any>>('Playlists/RemoveSubreddit', data).then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        })
    }
}