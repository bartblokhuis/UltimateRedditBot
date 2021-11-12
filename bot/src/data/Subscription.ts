import { Sort } from "./Sort";

export interface Subscription {
    id: string,
    lastPostId: string,
    sort: Sort,
    subredditName: string,
    channels: ChannelSubscription[]
};

export interface ChannelSubscription {
    id: string,
    discordChannelId: string,
    guildId: string,
    isEnabled: boolean,
    isShowNsfw: boolean
};
