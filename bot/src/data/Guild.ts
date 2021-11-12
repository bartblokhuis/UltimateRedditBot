import { GuildSetting } from "./GuildSetting";

export interface Guild {
    id: string,
    discordGuildId: string,
    setting: GuildSetting,
}

