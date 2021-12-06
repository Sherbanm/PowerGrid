import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {

  private messageSource = new BehaviorSubject<string>('');
  currentError = this.messageSource.asObservable();

  constructor() { }

  changeCurrentErrorFromResponse(obj: object) {
    var todo = obj as APIResponse;
    if (todo.success) {
      this.messageSource.next("success");
    }
    else {
      this.messageSource.next(todo.message);
    }
  }
}

interface APIResponse {
  success: boolean;
  message: string;
}


