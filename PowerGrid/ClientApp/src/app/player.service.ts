import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http'
import { Observable, of} from 'rxjs';

import { Player} from './domain/player';
import { PLAYERS} from './mock-players';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private playerUrl = 'https://localhost:44344/api/Player'

  constructor(private http: HttpClient) { }

  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(this.playerUrl);
  }
}
