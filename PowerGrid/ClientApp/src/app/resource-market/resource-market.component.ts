import { Component, OnInit, Input } from '@angular/core';
import { CurrentPlayerService } from '../current-player.service';
import { GamestateService } from '../gamestate.service';

import { ResourceType } from '../domain/resourceType';
import { Player } from '../domain/player';
import { ResourceMarket } from '../domain/resourceMarket';

@Component({
  selector: 'app-resource-market',
  templateUrl: './resource-market.component.html',
  styleUrls: ['./resource-market.component.css']
})
export class ResourceMarketComponent implements OnInit {
  @Input() market: ResourceMarket;
  
  public selectedPlayer: Player;

  constructor(private currentPlayerService: CurrentPlayerService, private gameStateService: GamestateService) { }

  ngOnInit() {
    this.currentPlayerService.currentPlayer.subscribe(player => this.selectedPlayer = player);
  }

  onBuyResource(type: ResourceType): void {
    this.gameStateService.buyResource(this.selectedPlayer, type, 1).subscribe(data => {
      let result = data;
    });
  }
}