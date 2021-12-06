import { Card } from "./card";
import { Player } from "./player";

export class AuctionHouse {
  drawpile: Array<Card>;
  marketplace: Array<Card>;
  playerswhopassedauction: Array<Player>;
  currentauctioncard: Card;
  currentbid: number;
  currentbidPlayer: Player;
}
