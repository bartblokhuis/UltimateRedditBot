import { Bot } from '../client/client';
import { Message } from 'discord.js'; 

export interface RunFunction {

    (client: Bot, message: Message, args: string[]): Promise<void>,
    
};

export interface Command {
    name: string,
    alias: string,
    category: string,
    run: RunFunction
}