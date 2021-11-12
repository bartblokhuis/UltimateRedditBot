import { Service } from "typedi";

@Service()
export class CacheService {

    cachedItems: CachedItem[] = [];

    get<T>(key: string, refreshLifetime: boolean = true) : T {

        const item = this.cachedItems.filter(item => item.key === key)[0]
        if(!item) return undefined;

        if(refreshLifetime) {
            item.experationTimeInMiliseconds = Date.now() + item.lifeTimeInMiliseconds;
            this.removeFromCache(key, item.lifeTimeInMiliseconds);
        }

        return item.data
    }

    put(key: string, data: any, lifeTimeInMiliseconds: number) {

        //If there already is a cached item with the key replace the data.
        const cachedItem = this.cachedItems.filter(item => item.key === key)[0];
        if(cachedItem) {
            cachedItem.data = data;
            cachedItem.experationTimeInMiliseconds = Date.now() + lifeTimeInMiliseconds;
        }
        else {
            this.cachedItems.push({ key: key, data: data, lifeTimeInMiliseconds: lifeTimeInMiliseconds, timeAddedInMiliseconds: Date.now(), experationTimeInMiliseconds: Date.now() + lifeTimeInMiliseconds })
        }

        this.removeFromCache(key, lifeTimeInMiliseconds);
    }

    private removeFromCache(key: string, lifeTimeInMiliseconds) {

        //When the life time in miliseconds has passed remove the item from the cache
        setTimeout(() => {
            const cachedItem = this.cachedItems.filter(item => item.key === key)[0];

            if(!cachedItem || cachedItem.experationTimeInMiliseconds > Date.now()) return; //Cached item either no longer exists or has been updated to stay in the cache for longer duration

            //Remove from the cached items
            this.cachedItems = this.cachedItems.filter(x => x.key !== key);

        }, lifeTimeInMiliseconds);
    }

}

interface CachedItem {
    key: string,
    data: any,
    lifeTimeInMiliseconds: number,
    timeAddedInMiliseconds: number,
    experationTimeInMiliseconds: number,
}