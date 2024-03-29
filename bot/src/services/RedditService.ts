import consola from 'consola';
import axios from 'axios'
import { RedditPost } from '../data/RedditPost';
import { Subreddit } from '../data/Subreddit';
import { Service } from 'typedi';
import { Sort } from '../data/Sort';
import { URLHelper } from '../utils/urlHelper';

@Service()
export class RedditService {
    getSubredditByName(subredditName: string) : Promise<Subreddit> {

        return axios.get(`https://www.reddit.com/subreddits/search.json?q=${subredditName.replace(/ /gi,'+')}&include_over_18=on&limit=1`)
            .then((result) => {
               try{
                   const data = result.data.data.children[0].data;
                   const subreddit : Subreddit = {name: data.display_name, isNsfw: data.over18, id: "", isBanned: false}
                   return subreddit;
               }
               catch{
                   return null;
               }
            })
            .catch((error) => {
                consola.error('Request failed with status code ' + error.response.status, error.response.config.url);
                return null;
            });
    };

    getPosts(subredditName: string, lastPostName: string, redditSort: Sort) : Promise<RedditPost[]> {
        const sortTime = this.parseSortToRedditSort(redditSort);

        let url = `https://reddit.com/r/${subredditName}/${sortTime.sort}/.json?limit=50`;
        if(sortTime.time){
            url += `&t=${sortTime.time}`;
        }
        if(lastPostName){
            url += `&after=${lastPostName}`;
        }

        return axios.get(url)
            .then((result) => {
               try{
                    const posts: RedditPost[] = [];
                    result.data.data.children.map(post => posts.push({ author: post.data.author, downs: post.data.downs, isOver18: post.data.over_18, postLink: `https://www.reddit.com${post.data.permaLink}`, selftext: post.data.selftext, thumbnail: post.data.thumbnail, title: post.data.title, ups: post.data.ups, url: post.data.url, postId: post.data.name, description: post.data.selftext, permalink: `https://www.reddit.com${post.data.permalink}`, stickied: post.data.stickied }));
                    return posts; //Ensure it has an image
               }
               catch{
                   return null;
               }
            })
            .catch((error) => {
                consola.error('Request failed with status code ' + error.response.status, error.response.config.url);
                return null;
            });
    }

    async getNewPost(subredditName: string, lastPostName: string, redditSort: Sort) : Promise<RedditPost> {

        const sortTime = this.parseSortToRedditSort(redditSort);

        let url = `https://reddit.com/r/${subredditName}/${sortTime.sort}/.json?limit=5`;
        if(sortTime.time){
            url += `&t=${sortTime.time}`;
        }

        return axios.get(url)
            .then((result) => {
               try{
                    let posts: RedditPost[] = [];
                    result.data.data.children.map(post => posts.push({ author: post.data.author, downs: post.data.downs, isOver18: post.data.over_18, postLink: `https://www.reddit.com${post.data.permaLink}`, selftext: post.data.selftext, thumbnail: post.data.thumbnail, title: post.data.title, ups: post.data.ups, url: post.data.url, postId: post.data.name, description: post.data.selftext, permalink: `https://www.reddit.com${post.data.permalink}`, stickied: post.data.stickied }));
            
                    //Remove the posts that don't have an image or are stickied
                    posts = posts.filter(x => x.stickied == false && URLHelper.isVideoOrImage(x.url));

                    //No new post has been found
                    if(posts.length === 0){
                        return undefined;
                    }

                    //No posts found
                    if(!posts || posts.length === 0){
                        return undefined;
                    }
                    
                    //No new post.
                    if(posts[0].postId == lastPostName){
                        return undefined;
                    }

                    return posts[0];
               }
               catch{
                   return null;
               }
            })
            .catch((error) => {
                consola.error('Request failed with status code ' + error.response.status, error.response.config.url);
                return null;
            });
    }

    private parseSortToRedditSort(sort: Sort) : SortTime {
        let sortTime: SortTime = { sort: '', time: ''};
        switch (sort){
            case Sort.hot: 
            sortTime.sort = 'hot';
            break;
            case Sort.new: 
            sortTime.sort = 'new';
            break;
            case Sort.topalltime: 
            sortTime.sort = 'top';
            sortTime.time = 'all'
            break;
            case Sort.topnow: 
            sortTime.sort = 'top';
            sortTime.time = 'hour'
            break;
            case Sort.topthismonth: 
            sortTime.sort = 'top';
            sortTime.time = 'month'
            break;
            case Sort.topthistear: 
            sortTime.sort = 'top';
            sortTime.time = 'year';
            break;
            case Sort.topthisweek: 
            sortTime.sort = 'top';
            sortTime.time = 'week';
            break;
            case Sort.toptoday: 
            sortTime.sort = 'top';
            sortTime.time = 'day'
            break;
        };

        return sortTime;
    }

}

interface SortTime{
    sort: string,
    time: string
};