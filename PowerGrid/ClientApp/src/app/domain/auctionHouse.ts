import { Card } from "./card";
import { Player } from "./player";

export class AuctionHouse {
  drawpile: Array<Card>;
  marketplace: Array<Card>;
  cardunderauction: Card;
  auctionpassedplayers: Array<Player>;
  currentbid: number;
  currentbidPlayer: Player;
}
