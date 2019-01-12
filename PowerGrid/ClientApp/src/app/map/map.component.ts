import { Component, OnInit, Input } from '@angular/core';
import { Map } from '../domain/map';
import { Player } from '../domain/player';
import { CurrentPlayerService } from '../current-player.service';
import { GamestateService } from '../gamestate.service';
import { City } from '../domain/city';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  @Input() map: Map;

  selectedPlayer: Player;

  constructor(private currentPlayerService: CurrentPlayerService, private gameStateService: GamestateService) { }

  ngOnInit() {
    this.currentPlayerService.currentPlayer.subscribe(player => this.selectedPlayer = player);
  }
  

  public onBuyGenerator(city: City): void {
    this.gameStateService.buyGenerator(this.selectedPlayer, city).subscribe(data => {
      let result = data;
    });
  }
}