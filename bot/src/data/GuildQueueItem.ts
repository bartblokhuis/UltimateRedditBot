import { Guild } from "./Guild";
import { QueueItem } from "./QueueItem";

export interface GuildQueueItem {
    guild: Guild,
    queueItems: QueueItem[],
    isRunning: boolean
}