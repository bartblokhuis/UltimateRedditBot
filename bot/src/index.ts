import 'reflect-metadata';
import { Config } from './interfaces/Config';
import * as File from '../config.json';
import { Bot } from './client/client';
import Container from 'typedi';
import { SubscriptionService } from './services/SubscriptionService';


new Bot().start((File as Config));