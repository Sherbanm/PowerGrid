import { Player } from './domain/player';
import { Card } from './domain/card';
import { ResourceType } from './domain/resourceType';

export const PLAYERS: Player[] = [
  {   
    name: 'Steve', 
    cards: [
      { minimumBid: 1, resource: ResourceType.Oil, generatorsPowered: 2, resourceCost: 3 },
      { minimumBid: 1, resource: ResourceType.Oil, generatorsPowered: 2, resourceCost: 3} ],
     money: 50, 
     resources: null,
     generators: 22 },

];
