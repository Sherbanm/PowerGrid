import { Component, OnInit } from '@angular/core';

import { WebSocketService } from '../websocket.service';
import { GameState } from '../domain/gamestate';
import { GamestateService } from '../gamestate.service';
import { CurrentPlayerService } from '../current-player.service';
import { Player } from '../domain/player';
import { ErrorHandlerService } from '../error-handler.service';

@Component({
  selector: 'app-gamestate',
  templateUrl: './gamestate.component.html',
  styleUrls: ['./gamestate.component.css']
})
export class GamestateComponent implements OnInit {

  gameState: GameState;

  selectedPlayer: Player;

  timeLeft: number;
  interval;

  constructor(private socketService: WebSocketService, private gameStateService: GamestateService, currentPlayerService: CurrentPlayerService, private errorHandlerService: ErrorHandlerService) {
    let _self = this;
    this.socketService.createObservableSocket( "wss://localhost:44344/ws").subscribe(data => {
      _self.gameState = JSON.parse(data);

      currentPlayerService.changeCurrentPlayer(_self.gameState.currentPlayer)
      this.timeLeft = _self.gameState.remainingTime;
      this.startTimer();
    });
  }

  ngOnInit() {
    this.socketService.sendMessage("hello");
  }

  startTimer() {
    clearInterval(this.interval);
    this.interval = setInterval(() => {
      if (this.timeLeft > .1) {
        this.timeLeft -= .1;
      }
      else {
        this.timeLeft = 0;
        clearInterval(this.interval);
      }
    }, 100);
  }

  onClickMe() {
    this.socketService.sendMessage("hello"); 
  }

  onAdvance() {
    this.gameStateService.advance().subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  onPassAuction() {
    this.gameStateService.auctionPassPhase(this.gameState.currentPlayer).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }
}
