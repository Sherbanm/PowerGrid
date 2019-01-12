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

  selectedPlayer: Player;

  constructor(private currentPlayerService: CurrentPlayerService, private gameStateService: GamestateService) { }

  ngOnInit() {
    this.currentPlayerService.currentPlayer.subscribe(player => this.selectedPlayer = player);
  }
  

  public onBuyCard(card: Card): void {
    this.gameStateService.buyCard(this.selectedPlayer, card).subscribe(data => {
      let result = data;
    });
  }
}