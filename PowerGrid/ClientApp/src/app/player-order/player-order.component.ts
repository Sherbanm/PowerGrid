import { Component, OnInit, Input } from '@angular/core';
import { Player } from '../domain/player';
import { Time } from '@angular/common';
import { CurrentPlayerService } from '../current-player.service';
import { timer } from 'rxjs'
import { GameState } from '../domain/gamestate';

@Component({
  selector: 'app-player-order',
  templateUrl: './player-order.component.html',
  styleUrls: ['./player-order.component.css']
})
export class PlayerOrderComponent implements OnInit {
  @Input() players: Player[];
  @Input() gameState: GameState;
  @Input() remainingSeconds: number;

  subscribeTimer: number;
  
  constructor(currentPlayerService: CurrentPlayerService) { 
    this.subscribeTimer = this.remainingSeconds;
    this.observableTimer();
  }

  ngOnInit() {
  }

  observableTimer() {
    const source = timer(1000, 1000);
    source.subscribe(val => {
      this.subscribeTimer = this.remainingSeconds - val;
    })
  }
}
