import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of} from 'rxjs';

import { ResourceType } from './domain/resourceType';
import { GameState } from './domain/gamestate';
import { Player } from './domain/player';


@Injectable({
  providedIn: 'root'
})
export class GamestateService {

  private playerUrl = 'https://localhost:44344/api/GameState/BuyResource';
  private playerUrl2 = 'https://localhost:44344/api/GameState';

  constructor(private http: HttpClient) { }

  getPlayers(): Observable<GameState> {
    return this.http.get<GameState>(this.playerUrl2);
  }

  buyResource(player: Player, type: ResourceType, count: number): Observable<string> {
    var parameter = JSON.stringify({ player, type, count});
    var options = { headers: new HttpHeaders({ 'Content-Type':  'application/json', 'Authorization': 'my-auth-token'})};
    return this.http.post<string>(this.playerUrl, parameter, options);
  }
}