import { Component, OnInit, Input } from '@angular/core';
import { Player } from '../domain/player';
import { CurrentPlayerService } from '../current-player.service';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.css']
})

export class PlayerComponent implements OnInit {
  @Input() player: Player;
  
  constructor(private currentPlayerService: CurrentPlayerService) { }

  ngOnInit() {
  }

  public getColor(player: Player) {
    return this.currentPlayerService.getColor(player);
  }

}
