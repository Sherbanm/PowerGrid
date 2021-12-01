import { Component, OnInit } from '@angular/core';

import { WebSocketService } from '../websocket.service';
import { GameState } from '../domain/gamestate';
import { GamestateService } from '../gamestate.service';
import { CurrentPlayerService } from '../current-player.service';
import { Player } from '../domain/player';

@Component({
  selector: 'app-gamestate',
  templateUrl: './gamestate.component.html',
  styleUrls: ['./gamestate.component.css']
})
export class GamestateComponent implements OnInit {

  gameState: GameState;

  selectedPlayer: Player;

  constructor(private socketService: WebSocketService, private gameStateService: GamestateService, private currentPlayerService: CurrentPlayerService) {
    let _self = this;
    this.socketService.createObservableSocket( "wss://localhost:44344/ws").subscribe(data => {
      _self.gameState = JSON.parse(data);
      currentPlayerService.changeCurrentPlayer(_self.gameState.currentPlayer)
    });
  }

  ngOnInit() {
    this.socketService.sendMessage("hello");
  }
  
  onClickMe() {
    this.socketService.sendMessage("hello"); 
  }

  onAdvance() {
    if (this.gameState.currentPhase == 1) {
      this.gameStateService.onPassAuction(this.gameState.currentPlayer).subscribe(data => {
        let result = data;
      });
    }
    else {
      this.gameStateService.onAdvance().subscribe(data => {
        let result = data;
      });
    }
  }


}
