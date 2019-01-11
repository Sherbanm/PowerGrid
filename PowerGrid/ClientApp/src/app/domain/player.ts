import { Card } from "./card";
import { Resources } from "./resources";

export class Player {
  name: string;
  cards: Card[];
  money: number;
  resources: Resources;
}
