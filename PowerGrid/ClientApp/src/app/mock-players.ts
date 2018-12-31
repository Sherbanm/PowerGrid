import { Player } from './domain/player';
import { Card } from './domain/card';

export const PLAYERS: Player[] = [
  { name: 'Steve', cards: [ 
    { value: 1, resource: "oil", power:  2, cost: 3},
    { value: 1, resource: "oil", power:  2, cost: 3} ] },
  { name: 'Narco', cards: [
    { value: 1, resource: "oil", power:  2, cost: 3}
  ] },
  { name: 'Bombasto', cards: [
    { value: 1, resource: "oil", power:  2, cost: 3},
    { value: 1, resource: "oil", power:  2, cost: 3},
    { value: 1, resource: "oil", power:  2, cost: 3}
  ] },
  { name: 'asdasd', cards: [
    { value: 1, resource: "oil", power:  2, cost: 3},
    { value: 1, resource: "oil", power:  2, cost: 3},
    { value: 1, resource: "oil", power:  2, cost: 3}
  ] }
];