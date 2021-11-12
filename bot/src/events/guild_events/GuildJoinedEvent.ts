import { Guild, Message } from 'discord.js';
import { readSync } from 'fs';
import { Command } from '../../interfaces/Command';
import { RunFunction } from '../../interfaces/Event';

export const run: RunFunction = async(client, guild: Guild) => {

}

export const name: string = 'guildCreate';