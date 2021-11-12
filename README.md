# Ultimate Reddit Bot

## General info
This is a discord bot that allows users to get posts from reddit, it is also possible to automatically get new posts from a subreddit.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Bot Commands](#commands)
	
## Technologies
Project is created with:
* .NET Core 6.0 (API)
* Typescript 4.3.5 (bot)
* Discord.js 12.5.3 (bot)
* Angular 12 (website)
	
## Setup

### Api
To run this project, update the API's connection string in the appsettings.json it is also highly recommended to change the API key.
Now you can start the API. Ensure that the API is working by going to /swagger/index.html this should open the swagger page.


### Bot
After launching the API configure the bot in the config.json
```
token: your discord bot token.
baseUrl: api url + /api/
superUserId: this user id always has mod permissions, If you don't want a superuser leave it empty or enter your superuser's discord id
apiKey: If you changed the apiKey in the API config you will have to update it here.
inviteUrl: enter your bot invite url here, this will be used for the $invite command.

```
After configuring the bot you can start it by running the following command:
```
$ yarn test
```
You should now  be able to see the bot online in discord.

### Website
To run the website ensure that the API is running, change the configurations in the environment.ts
```
baseUri: api url + /api/Statistics/
apiKey: If you changed the apiKey in the API config you will have to update it here.
```

## Bot Commands
You can view the bot commands by running the $help command or by visiting the bot website here: https://www.ultimateredditbot.com/
