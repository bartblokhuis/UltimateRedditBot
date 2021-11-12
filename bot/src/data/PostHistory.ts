import { RedditSort } from "./RedditSort";

export interface PostHistory {
    lastPostId: string;
    sort: RedditSort;
    subredditId: string;
    guildId: string;
}