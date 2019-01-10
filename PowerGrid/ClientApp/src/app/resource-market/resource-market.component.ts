import { Component, OnInit, Input } from '@angular/core';
import { ResourceMarket } from '../domain/resourceMarket';
import { ResourceType } from '../domain/resourceType';
import { CurrentPlayerServiceService } from '../current-player-service.service';
import { Player } from '../domain/player';
import { PlayerService } from '../player.service';

@Component({
  selector: 'app-resource-market',
  templateUrl: './resource-market.component.html',
  styleUrls: ['./resource-market.component.css']
})
export class ResourceMarketComponent implements OnInit {
  @Input() market: ResourceMarket;
  
  public selectedPlayer: Player;

  constructor(private currentPlayerServiceService: CurrentPlayerServiceService, private playerSerivce: PlayerService) { }

  ngOnInit() {
    this.currentPlayerServiceService.currentPlayer.subscribe(player => this.selectedPlayer = player);
  }

  onBuyResource(type: ResourceType): void {
    this.playerSerivce.buyResource(this.selectedPlayer, type, 1).subscribe(data => {
      let result = data;
    });
  }
}