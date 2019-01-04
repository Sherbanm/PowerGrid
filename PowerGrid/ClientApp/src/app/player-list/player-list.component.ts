import { Component, OnInit, Input } from '@angular/core';

import { Player} from '../domain/player';
import { PlayerService } from '../player.service';
import { SocketService } from '../socket.service';
import { GameState } from '../domain/gamestate';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent implements OnInit {
  @Input() players: Player[];
  
  selectedPlayer: Player;

  gameState: GameState;

  constructor(private playerService: PlayerService) {
  }

  ngOnInit() {
    this.getPlayers();
  }

  onSelect(player: Player): void {
    this.selectedPlayer = player;
  }
  
  getPlayers(): void {
    this.playerService.getPlayers().subscribe(players => {
      this.players = players;
      this.selectedPlayer = players[0];
    });
  }

}
