import { Injectable } from '@angular/core';

import { Observable, of} from 'rxjs';

import { Player} from './domain/player';
import { PLAYERS} from './mock-players';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  constructor() { }

  getPlayers(): Observable<Player[]> {
    return of(PLAYERS);
  }
}
