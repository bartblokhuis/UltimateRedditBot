import { Service } from 'typedi';
import { Result } from '../data/Result';
import { User } from '../data/User';
import { BaseApiService } from './BaseApiService';


@Service()
export class UserService extends BaseApiService{

    cachedApiUsers: User[] = [];

    constructor(){
        super();
    }

    /**
     * Get a user from the API
     * @param discordId the discord user id
     * @returns 
     */
    async getUser(discordId: string) : Promise<User>{

        //Try getting from cache
        let user = this.cachedApiUsers.filter(apiUser => apiUser.discordUserId == discordId)[0];
        if(user) return user; //Found in cache.

        return this.get<Result<User>>(`User?discordUserId=${discordId}`)
        .then((resuolt) => {
            return resuolt.data.data;
        }).catch(() => {
            return undefined;
        });
    };
}