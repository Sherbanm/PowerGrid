import { Component, OnInit, Input } from '@angular/core';

import { AuctionHouse } from '../domain/auctionHouse';
import { Player } from '../domain/player';
import { CurrentPlayerService } from '../current-player.service';
import { GamestateService } from '../gamestate.service';
import { Card } from '../domain/card';
import { ErrorHandlerService } from '../error-handler.service';

@Component({
  selector: 'app-auction-house',
  templateUrl: './auction-house.component.html',
  styleUrls: ['./auction-house.component.css']
})
export class AuctionHouseComponent implements OnInit {
  @Input() auctionHouse: AuctionHouse;
  @Input() currentBidder: Player;


  constructor(private gameStateService: GamestateService, private errorHandlerService: ErrorHandlerService) { }

  ngOnInit() {
  }

  bid(event) {
    this.gameStateService.auctionBid(this.currentBidder, this.auctionHouse.currentbid + 1).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  pass(event) {
    this.gameStateService.auctionPassCard(this.currentBidder).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  public onSetAuctionedCard(card: Card): void {
    if (this.auctionHouse.cardunderauction == null) {
      this.gameStateService.auctionSetCard(card, this.currentBidder).subscribe(data => {
        this.errorHandlerService.changeCurrentErrorFromResponse(data);
      });
    }
    else if (this.auctionHouse.cardunderauction.minimumBid != card.minimumBid) {
      this.gameStateService.auctionSetCard(card, this.currentBidder).subscribe(data => {
        this.errorHandlerService.changeCurrentErrorFromResponse(data);
      });
    }
  }

}
