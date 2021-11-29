import { Injectable } from '@angular/core';
import { BehaviorSubject} from 'rxjs/internal/BehaviorSubject';
import { Player } from './domain/player';

@Injectable({
  providedIn: 'root'
})
export class CurrentPlayerService {

  private messageSource = new BehaviorSubject<Player>(null);
  currentPlayer = this.messageSource.asObservable();
  currentBidder = this.messageSource.asObservable();

  constructor() { }

  changeCurrentPlayer(player: Player) {
    this.messageSource.next(player);
  }

  getColor(player: Player): string {
    switch(player.name) {
      case "Albert":
        return "#ffcccc";
      case "Bruce":
        return "#99ccff";
      case "Charles":
        return "#ffffcc";
      case "Dan":
        return "#ccffcc";
      case "Eustache":
        return "#e6ccff";
      case "Franz":
        return "#e0e0eb";
    }
  }
}
