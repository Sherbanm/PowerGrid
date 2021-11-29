import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuctionHouse } from '../domain/auctionHouse';
import { Card} from '../domain/card';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})

export class CardComponent implements OnInit {
  @Input() card: Card;
  @Input() auctionHouse: AuctionHouse;

  @Output() bidEvent = new EventEmitter<Card>();
  @Output() passEvent = new EventEmitter<Card>();

  constructor() { }

  ngOnInit() {
  }

  public onBid(): void {
    this.bidEvent.emit(this.card)
  }

  public onPass(): void {
    this.passEvent.emit(this.card)
  }

}
