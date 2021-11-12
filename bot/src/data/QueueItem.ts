import { TextChannel } from "discord.js";
import { Sort } from "./Sort";
import { Subreddit } from "./Subreddit";

export interface QueueItem {
    subreddit: Subreddit,
    channel: TextChannel,
    amountOfPosts: number,
    lastUsedPostName: string,
    failedGetAttemppts: number,
    sort: Sort,
    guildId: string,
    requestedBy: string
}