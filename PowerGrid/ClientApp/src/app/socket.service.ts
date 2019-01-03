import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GameState } from './domain/gamestate';

@Injectable({
  providedIn: 'root'
})
export class SocketService {
  ws: WebSocket;

  createObservableSocket(url:string):Observable<GameState> {
    this.ws = new WebSocket(url);
    return new Observable(observer => {
      this.ws.onmessage = (event) => {
        observer.next(event.data)
      };
      this.ws.onerror = (event) => observer.error(event);
      this.ws.onclose = (event) => observer.complete();
    });
  }

  sendMessage(message: any) { this.ws.send(message);}
}
