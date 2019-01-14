import { Card } from "./card";
import { Resources } from "./resources";

export class Player {
  name: string;
  cards: Array<Card>;
  money: number;
  resources: Resources;
  generators: number;
}
