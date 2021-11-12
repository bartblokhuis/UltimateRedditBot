import { Component, OnInit } from '@angular/core';
import { CommandGroup } from 'src/app/data/command';

@Component({
  selector: 'app-commands',
  templateUrl: './commands.component.html',
  styleUrls: ['./commands.component.scss']
})
export class CommandsComponent implements OnInit {

  commandGroups: CommandGroup[] = [
    { name: 'Reddit', commands: [ 
      {name: 'Get a post', command: '$(subreddit name) (sorting method) (amount of posts)', description: 'This command can be used to get post(s) from a subreddit. The sorting method and the amount of posts are both optional. The default sorting method is configurable in the settings, the default amount of posts is 1.', usage: '', modOnly: true, ownerOnly: true } 
    ] },
    { name: 'Playlists', commands: [ 
      {name: 'Post', command: '$p (playlist name)', description: 'Gets a post from a subreddit in the playlist.', usage: '', modOnly: true, ownerOnly: true },
      {name: 'Playlists', command: '$p-info', description: 'Shows the playlists in the server.', usage: '', modOnly: false, ownerOnly: false },
      {name: 'Subreddits in playlist', command: '$p-info (playlist name)', description: 'Shows the subreddits in the playlist.', usage: '', modOnly: false, ownerOnly: false },
      {name: 'Add playlist', command: '$p-add (name)', description: 'Creates a new playlist.', usage: '', modOnly: true, ownerOnly: true },
      {name: 'Add subreddit to playlist', command: '$p-add (playlist name) (subreddit name)', description: 'Adds a subreddit to a playlist.', usage: '', modOnly: true, ownerOnly: true },
      {name: 'Remove playlist', command: ' $p-remove (name)', description: 'Removes a playlist.', usage: '', modOnly: true, ownerOnly: true },
      {name: 'Remove subreddit from a playlist', command: '$p-remove (playlist name) (subreddit name)', description: 'Removes a subreddit from a playlist.', usage: '', modOnly: true, ownerOnly: true },
     ] },
    { name: 'Mods', commands: [
       {name: 'Mod', command: '$mod (tag user)', description: 'Makes the tagged user a mod', usage: '', modOnly: true, ownerOnly: true },
       {name: 'Unmod', command: '$unmod (tag user)', description: 'Removes the mod permissions from the tagged user', usage: '', modOnly: true, ownerOnly: true },
      ] },
    { name: 'Banned subreddits', commands: [ 
      {name: 'Ban', command: '$ban (subreddit)', description: 'Bans the subreddit', usage: '', modOnly: true, ownerOnly: true },
      {name: 'Unban', command: '$unban (subreddit)', description: 'Unbans the subreddit', usage: '', modOnly: true, ownerOnly: true },
     ] },
    { name: 'Settings', commands: [ 
      {name: 'Enabled', command: ' $enabled (true/false)', description: 'When the bot is disabled it will only listen to the command to enable it again. Subscriptions will also be stopped till the bot gets enabled again.', usage: '', modOnly: true, ownerOnly: true },
      {name: 'Allow nsfw', command: '$allow-nsfw (true/false)', description: 'When allow-nsfw is turned off posts with the nsfw tag will be ignored.', usage: '', modOnly: true, ownerOnly: true },
      {name: 'Prefix', command: '$prefix', description: "Used to change the bot's prefix", usage: '', modOnly: true, ownerOnly: true },
      {name: 'Sort', command: ' $sort (new sorting method)', description: 'Used to change the default sorting method, the following sorting methods are supported: Hot, New, Random, topnow, toptody, topthisweek, topthismonth, topthisyear, topalltime', usage: '', modOnly: true, ownerOnly: true },
     ] },
  ]
  constructor() { }

  ngOnInit(): void {
  }

}
