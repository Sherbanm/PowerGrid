import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuctionHouse } from '../domain/auctionHouse';
import { Card} from '../domain/card';
import { ErrorHandlerService } from '../error-handler.service';
import { GamestateService } from '../gamestate.service';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})

export class CardComponent implements OnInit {
  @Input() card: Card;
  @Input() auctionHouse: AuctionHouse;
  @Input() owned: boolean;

  @Output() bidEvent = new EventEmitter<Card>();
  @Output() passEvent = new EventEmitter<Card>();

  constructor(private gameStateService: GamestateService, private errorHandlerService: ErrorHandlerService) { }

  ngOnInit() {
  }

  public onBid(): void {
    this.bidEvent.emit(this.card)
  }

  public onPass(): void {
    this.passEvent.emit(this.card)
  }

  public onLoadResource(): void {
    this.gameStateService.loadResource(this.card, this.card.resource).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  public onLoadGas(): void {
    this.gameStateService.loadResource(this.card, 2).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  public onLoadOil(): void {
    this.gameStateService.loadResource(this.card, 1).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }
}
