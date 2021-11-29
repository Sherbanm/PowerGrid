import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable} from 'rxjs';

import { ResourceType } from './domain/resourceType';
import { GameState } from './domain/gamestate';
import { Player } from './domain/player';
import { Card } from './domain/card';
import { City } from './domain/city';


@Injectable({
  providedIn: 'root'
})
export class GamestateService {

  private url = 'https://localhost:44344/api/GameState';
  

  constructor(private http: HttpClient) { }

  getPlayers(): Observable<GameState> {
    return this.http.get<GameState>(this.url);
  }

  buyResource(player: Player, type: ResourceType, count: number): Observable<string> {
    var parameter = JSON.stringify({ player, type, count});
    var options = { headers: new HttpHeaders({ 'Content-Type':  'application/json', 'Authorization': 'my-auth-token'})};
    return this.http.post<string>(this.url + "/BuyResource", parameter, options);
  }

  buyCard(player: Player, card: Card): Observable<string> {
    var parameter = JSON.stringify({ player, card});
    var options = { headers: new HttpHeaders({ 'Content-Type':  'application/json', 'Authorization': 'my-auth-token'})};
    return this.http.post<string>(this.url + "/BuyCard", parameter, options);
  }

  bid(player: Player, amount: number): Observable<string> {
    var parameter = JSON.stringify({ player, amount });
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<string>(this.url + "/Bid", parameter, options);
  }

  pass(player: Player): Observable<string> {
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<string>(this.url + "/Pass", player, options);
  }

  setAuctionedCard(card: Card, player : Player): Observable<string> {
    var parameter = JSON.stringify({ card, player });
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<string>(this.url + "/SetAuctionedCard", parameter, options);
  }

  buyGenerator(player: Player, city: City): Observable<string> {
    var parameter = JSON.stringify({ player, city});
    var options = { headers: new HttpHeaders({ 'Content-Type':  'application/json', 'Authorization': 'my-auth-token'})};
    return this.http.post<string>(this.url + "/BuyGenerator", parameter, options);
  }

  onAdvance() {
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<string>(this.url + "/Advance", options);
  }
}
