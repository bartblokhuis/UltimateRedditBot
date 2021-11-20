import consola, { Consola } from 'consola';
import { Client, MessageEmbedOptions, MessageEmbed, Message, Intents, Collection, ChannelManager } from 'discord.js';
import glob from 'glob';
import { Service } from 'typedi';
import { promisify } from 'util';
import { Command } from '../interfaces/Command';
import { Config } from '../interfaces/Config';
import { Event } from '../interfaces/Event';

const globalPromise = promisify(glob);

@Service()
class Bot extends Client {

    public logger: Consola = consola;
    public commands: Command[] = new Array();
    public events: Collection<string, Event> = new Collection();
    public config: Config;
    public joinedGuildIds: string[];
    declare public channels: ChannelManager;
    
    public constructor(){
        super({ ws: { intents: Intents.ALL }, messageCacheLifetime: 100, messageCacheMaxSize: 200, messageEditHistoryMaxSize: 200, messageSweepInterval: 180});
    };

    public async start(config: Config) : Promise<void> {
        this.config = config;

        //Reguster the events
        const eventFiles: string[] = await globalPromise(`${__dirname}/../events/**/*{.ts,.js}`);
        eventFiles.map(async(value: string) => {
            const file: Event = await import(value);
            this.events.set(file.name, file);
            this.on(file.name, file.run.bind(null, this));
        });

        //Register the commands
        const commandFiles: string[] = await globalPromise(`${__dirname}/../commands/**/*{.ts,.js}`);
        commandFiles.map(async(value: string) => {
            const file: Command = await import(value);
            this.commands.push(file);
        });
        this.logger.info("Bot commands and events have been registered.")

        //Sign in
        await this.login(config.token);
        this.logger.success("The bot has signed in.")
    }

    public embed(options: MessageEmbedOptions, message: Message): MessageEmbed {
        return new MessageEmbed({...options, color: 'RANDOM'}).setFooter(`${message.author.tag} | ${this.user.username}`, message.author.displayAvatarURL({format: 'png', dynamic: true}))
    }
};

export { Bot };