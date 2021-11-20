import Container from 'typedi';
import { RunFunction } from '../../interfaces/Event';
import { SubscriptionService } from '../../services/SubscriptionService';

export const run: RunFunction = async (client) => {
    //Start the subscription service.
    const subscriptionService = Container.get(SubscriptionService);
    subscriptionService.start();
}

export const name: string = 'ready';