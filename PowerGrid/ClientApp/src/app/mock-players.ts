import { Player } from './domain/player';
import { Card } from './domain/card';
import { ResourceType } from './domain/resourceType';

export const PLAYERS: Player[] = [
  {   
    name: 'Steve', 
    cards: [ 
      { value: 1, resource: ResourceType.Oil, power:  2, cost: 3},
      { value: 1, resource: ResourceType.Oil, power:  2, cost: 3} ],
     money: 50, 
     resources: null,
     generators: 22 },

];