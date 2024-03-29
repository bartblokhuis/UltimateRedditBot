import { RunFunction } from '../../interfaces/Command';
import { Config } from '../../interfaces/Config';
import * as File from '../../../config.json';

export const run: RunFunction = async(client, message, args) => {
    const config = (File as Config);
    message.channel.send(config.inviteUrl)
};

export const name: string = 'Invite';
export const alias: string = 'inv';