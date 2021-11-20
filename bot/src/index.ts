import 'reflect-metadata';
import { Config } from './interfaces/Config';
import * as File from '../config.json';
import { Bot } from './client/client';

new Bot().start((File as Config));