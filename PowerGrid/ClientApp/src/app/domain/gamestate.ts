import { Player } from "./player";
import { Resources } from "./resources";
import { AuctionHouse } from "./auctionHouse";
import { Map } from "./map";

export class GameState {
    players: Player[];
    resourceMarket: Resources;
    auctionHouse: AuctionHouse;
    map: Map;
    
}