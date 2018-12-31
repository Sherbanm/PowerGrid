import { Component, OnInit } from '@angular/core';

import { Player} from '../domain/player';
import { PlayerService } from '../player.service';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent implements OnInit {

  players: Player[];
  selectedPlayer: Player;

  constructor(private playerService: PlayerService ) {

  }

  ngOnInit() {
    this.getPlayers();
    this.selectedPlayer = this.players[0];
  }

  onSelect(player: Player): void {
    this.selectedPlayer = player;
  }

  getPlayers(): void {
    this.playerService.getPlayers().subscribe(players => this.players = players);
  }

}
