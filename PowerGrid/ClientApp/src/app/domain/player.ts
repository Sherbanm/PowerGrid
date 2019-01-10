import { Card } from "./card";
import { ResourceMarket } from "./resourceMarket";

export class Player {
  name: string;
  cards: Card[];
  money: number;
  resources: ResourceMarket;
}
