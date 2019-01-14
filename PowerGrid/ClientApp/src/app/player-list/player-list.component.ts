import { Component, OnInit, Input } from '@angular/core';

import { Player} from '../domain/player';
import { CurrentPlayerService } from '../current-player.service';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent  {
  @Input() players: Player[];
  
  selectedPlayer: Player;

  constructor(private currentPlayerService: CurrentPlayerService) {
  }

  ngOnInit() {
    let _self = this;
    this.currentPlayerService.currentPlayer.subscribe(player => _self.selectedPlayer = player);
    this.selectedPlayer = this.players[0];
  }

  onSelect(player: Player): void {
    this.selectedPlayer = player;
    this.currentPlayerService.changeCurrentPlayer(player);
  }
}
