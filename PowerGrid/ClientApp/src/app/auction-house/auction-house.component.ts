import { Component, OnInit, Input } from '@angular/core';

import { AuctionHouse } from '../domain/auctionHouse';
import { Player } from '../domain/player';
import { CurrentPlayerService } from '../current-player.service';
import { GamestateService } from '../gamestate.service';
import { Card } from '../domain/card';

@Component({
  selector: 'app-auction-house',
  templateUrl: './auction-house.component.html',
  styleUrls: ['./auction-house.component.css']
})
export class AuctionHouseComponent implements OnInit {
  @Input() auctionHouse: AuctionHouse;
  @Input() currentBidder: Player;


  constructor(private gameStateService: GamestateService) { }

  ngOnInit() {
  }

  public onBuyCard(card: Card): void {
    this.gameStateService.buyCard(this.currentBidder, card).subscribe(data => {
      let result = data;
    });
  }

  bid(event) {
    this.gameStateService.bid(this.currentBidder, this.auctionHouse.currentbid + 1).subscribe(data => {
      let result = data;
    });
  }

  pass(event) {
    this.gameStateService.pass(this.currentBidder).subscribe(data => {
      let result = data;
    });
  }

  public onSetAuctionedCard(card: Card): void {
    if (this.auctionHouse.cardunderauction == null) {
      this.gameStateService.setAuctionedCard(card, this.currentBidder).subscribe(data => {
        let result = data;
      });
    }
    else if (this.auctionHouse.cardunderauction.minimumBid != card.minimumBid) {
      this.gameStateService.setAuctionedCard(card, this.currentBidder).subscribe(data => {
        let result = data;
      });
    }
  }

}
