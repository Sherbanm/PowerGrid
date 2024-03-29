import { Player } from "./player";
import { ResourceMarket } from "./resourceMarket";
import { AuctionHouse } from "./auctionHouse";
import { Map } from "./map";

export class GameState {
  players: Player[];
  playerOrder: Player[];
  resourceMarket: ResourceMarket;
  auctionHouse: AuctionHouse;
  map: Map;
  currentPlayer: Player;
  currentBidder: Player;
  remainingTime: number;
  currentPhase: number;
  currentStep: number;
}
