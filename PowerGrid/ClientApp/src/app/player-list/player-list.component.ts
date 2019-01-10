import { Component, OnInit, Input } from '@angular/core';

import { Player} from '../domain/player';
import { CurrentPlayerServiceService } from '../current-player-service.service';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent  {
  @Input() players: Player[];
  
  selectedPlayer: Player;

  constructor(private currentPlayerServiceService: CurrentPlayerServiceService) {
  }

  ngOnInit() {
    this.currentPlayerServiceService.currentPlayer.subscribe(player => this.selectedPlayer = player);
  }

  onSelect(player: Player): void {
    this.selectedPlayer = player;
    this.currentPlayerServiceService.changeCurrentPlayer(player);
  }
}
