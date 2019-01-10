import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse} from '@angular/common/http'
import { Observable, of} from 'rxjs';

import { Player} from './domain/player';
import { ResourceType } from './domain/resourceType';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private playerUrl = 'https://localhost:44344/api/GameState/BuyResource';
  private playerUrl2 = 'https://localhost:44344/api/GameState';

  constructor(private http: HttpClient) { }

  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(this.playerUrl2);
  }

  buyResource(player: Player, type: ResourceType, count: number): Observable<string> {
    var parameter = JSON.stringify({ player, type, count});
    var options = { headers: new HttpHeaders({ 'Content-Type':  'application/json', 'Authorization': 'my-auth-token'})};
    return this.http.post<string>(this.playerUrl, parameter, options);
  }
}
