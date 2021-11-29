import { Card } from './domain/card';
import { ResourceType } from './domain/resourceType';

export const CARDS: Card[] = [
  { minimumBid: 1, resource: ResourceType.Coal, generatorsPowered:  2, resourceCost: 3},
  { minimumBid: 1, resource: ResourceType.Gas, generatorsPowered: 2, resourceCost: 3},
  { minimumBid: 1, resource: ResourceType.Mixed, generatorsPowered: 2, resourceCost: 3},
];
