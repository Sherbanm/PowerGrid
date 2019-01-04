import { Component, OnInit } from '@angular/core';

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

  players: Player[];
  selectedPlayer: Player;

  gameState: GameState;

  constructor(private playerService: PlayerService, private socketService: SocketService) {
    let _self = this;
    this.socketService.createObservableSocket( "wss://localhost:44344/ws").subscribe(data => {
      _self.gameState = JSON.parse(data);
      _self.players = _self.gameState.players;
    });
  }

  ngOnInit() {
    this.getPlayers();
  }

  onSelect(player: Player): void {
    this.selectedPlayer = player;
    this.sendMessage("chacha");
  }

  public sendMessage(message: string): void {
    if (!message) {
      return;
    }

    this.socketService.sendMessage(message);    
  }


  getPlayers(): void {
    this.playerService.getPlayers().subscribe(players => {
      this.players = players;
      this.selectedPlayer = players[0];
    });
  }

}
