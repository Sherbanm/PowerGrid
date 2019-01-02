import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SocketService {
  ws: WebSocket;

  createObservableSocket(url:string):Observable<string> {
    this.ws = new WebSocket(url);
    return new Observable(observer => {
      this.ws.onmessage = (event) => observer.next(event.data);
      this.ws.onerror = (event) => observer.error(event);
      this.ws.onclose = (event) => observer.complete();
    });
  }

  sendMessage(message: any) { this.ws.send(message);}
}
