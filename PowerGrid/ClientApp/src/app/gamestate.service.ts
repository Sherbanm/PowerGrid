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

  advance() {
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<object>(this.url + "/Advance", options);
  }

  auctionSetCard(card: Card, player: Player): Observable<object> {
    var parameter = JSON.stringify({ card, player });
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<object>(this.url + "/AuctionSetCard", parameter, options);
  }

  auctionBid(player: Player, amount: number): Observable<object> {
    var parameter = JSON.stringify({ player, amount });
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<object>(this.url + "/AuctionBid", parameter, options);
  }

  auctionPassCard(player: Player): Observable<object> {
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<object>(this.url + "/AuctionPassCard", player, options);
  }

  auctionPassPhase(player: Player): Observable<object>  {
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<object>(this.url + "/AuctionPassPhase", player, options);
  }

  buyResource(player: Player, type: ResourceType, count: number): Observable<object> {
    var parameter = JSON.stringify({ player, type, count});
    var options = { headers: new HttpHeaders({ 'Content-Type':  'application/json', 'Authorization': 'my-auth-token'})};
    return this.http.post<object>(this.url + "/BuyResource", parameter, options);
  }

  buyGenerator(player: Player, city: City): Observable<object> {
    var parameter = JSON.stringify({ player, city});
    var options = { headers: new HttpHeaders({ 'Content-Type':  'application/json', 'Authorization': 'my-auth-token'})};
    return this.http.post<object>(this.url + "/BuyGenerator", parameter, options);
  }

  loadResource(card: Card, resourceType: number): Observable<object> {
    var parameter = JSON.stringify({ card, resourceType });
    var options = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'my-auth-token' }) };
    return this.http.post<object>(this.url + "/LoadResource", parameter, options);
  }
}
