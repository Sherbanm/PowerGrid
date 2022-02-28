import { Component, OnInit, Input } from '@angular/core';
import { Player } from '../domain/player';
import { GameState } from '../domain/gamestate';

@Component({
  selector: 'app-player-order',
  templateUrl: './player-order.component.html',
  styleUrls: ['./player-order.component.css']
})
export class PlayerOrderComponent implements OnInit {
  @Input() players: Player[];
  @Input() gameState: GameState;
  
  constructor() {
  }

  ngOnInit() {
  }
}
