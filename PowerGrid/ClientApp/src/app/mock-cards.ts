import { Card } from './domain/card';
import { ResourceType } from './domain/resourceType';

export const CARDS: Card[] = [
    { value: 1, resource: ResourceType.Coal, power:  2, cost: 3},
    { value: 1, resource: ResourceType.Gas, power:  2, cost: 3},
    { value: 1, resource: ResourceType.Mixed, power:  2, cost: 3},
];