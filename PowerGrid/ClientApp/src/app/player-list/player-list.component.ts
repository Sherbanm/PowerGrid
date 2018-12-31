import { Player} from '../domain/player';
import { PLAYERS } from '../mock-players';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent implements OnInit {

  players = PLAYERS;
  selectedPlayer: Player;

  constructor() { }

  ngOnInit() {
  }

  onSelect(player: Player): void {
    this.selectedPlayer = player;
  }

}
