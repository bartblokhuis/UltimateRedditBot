import { Subreddit } from "./Subreddit";

export interface Playlist {
    id: string,
    name: string,
    isGlobal: boolean,
    guildId: string,
    subreddits: Subreddit[]
}