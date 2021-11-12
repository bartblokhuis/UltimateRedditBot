import { Service } from "typedi";
import { Result } from "../data/Result";
import { BaseApiService } from "./BaseApiService";


@Service()
export class GuildAdminService extends BaseApiService{
    
    constructor(){
        super();
    }

    /**
     * Used to check if the user is an admin in the guild.
     * @param userId The user id
     * @param guildId The guild id
     * @returns true if the user is an admin
     */
    async isAdmin(userId: string, guildId: string) : Promise<boolean> {

        return this.get<Result<boolean>>(`GuildAdmins?userId=${userId}&guildId=${guildId}`)
            .then((result) => {
                return result.data.data;
            }).catch(() => {
                return undefined;
            });
    }

    /**
     * Used to give a user admin privelliges in a guild.
     * @param userId The user id
     * @param guildId The guild id
     * @returns 
     */
    async addAdmin(userId: string, guildId: string): Promise<Result<any>> {
        const data = { userId: userId, guildId: guildId };

        return this.post<Result<any>>('GuildAdmins/AddAdmin', JSON.stringify(data))
        .then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        });
    }

    /**
     * Used to remove a users admin privelliges in a guild.
     * @param userId The user id
     * @param guildId The guild id
     * @returns 
     */
    async removeAdmin(userId: string, guildId: string): Promise<Result<any>> {
        const data = { userId: userId, guildId: guildId };
        return this.post<Result<any>>('GuildAdmins/RemoveAdmin', JSON.stringify(data))
        .then((result) => {
            return result.data;
        }).catch(() => {
            return undefined;
        });
    }
}