import { Component } from '@angular/core';
import { ErrorHandlerService } from './error-handler.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  lastError;

  constructor(private errorHandlerService: ErrorHandlerService) { }

  ngOnInit() {
    this.errorHandlerService.currentError.subscribe(error =>this.lastError = error);
  }
}
