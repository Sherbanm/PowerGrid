import { Player } from "./player";
import { ResourceMarket } from "./resourceMarket";
import { AuctionHouse } from "./auctionHouse";
import { Map } from "./map";

export class GameState {
  players: Player[];
  resourceMarket: ResourceMarket;
  auctionHouse: AuctionHouse;
  map: Map;
  currentPlayer: Player;
  currentBidder: Player;
  remainingSeconds: number;
}
