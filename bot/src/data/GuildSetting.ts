import { Sort } from "./Sort";

export interface GuildSetting {
    prefix: string,
    allowNsfw: boolean,
    isEnabled: boolean,
    maxQueueItems: number,
    maxPlaylists: number,
    maxSubscriptions: number,
    sort: Sort
};
