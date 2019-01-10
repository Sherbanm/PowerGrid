import { Injectable } from '@angular/core';
import { BehaviorSubject} from 'rxjs/internal/BehaviorSubject';
import { Player } from './domain/player';

@Injectable({
  providedIn: 'root'
})
export class CurrentPlayerServiceService {

  private messageSource = new BehaviorSubject<Player>(null);
  currentPlayer = this.messageSource.asObservable();

  constructor() { }

  changeCurrentPlayer(player: Player) {
    this.messageSource.next(player);
  }
}
