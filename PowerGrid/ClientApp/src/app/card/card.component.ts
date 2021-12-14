import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CurrentPlayerService } from '../current-player.service';
import { AuctionHouse } from '../domain/auctionHouse';
import { Card} from '../domain/card';
import { Player } from '../domain/player';
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

  selectedPlayer: Player;

  constructor(private gameStateService: GamestateService, private errorHandlerService: ErrorHandlerService, private currentPlayerService: CurrentPlayerService) { }

  ngOnInit() {
    this.currentPlayerService.currentPlayer.subscribe(player => this.selectedPlayer = player);
  }

  public onBid(): void {
    this.bidEvent.emit(this.card)
  }

  public onPass(): void {
    this.passEvent.emit(this.card)
  }

  public onLoadResource(): void {
    this.gameStateService.loadResource(this.selectedPlayer, this.card, this.card.resource).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  public onLoadGas(): void {
    this.gameStateService.loadResource(this.selectedPlayer, this.card, 1).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  public onLoadOil(): void {
    this.gameStateService.loadResource(this.selectedPlayer, this.card, 2).subscribe(data => {
      this.errorHandlerService.changeCurrentErrorFromResponse(data);
    });
  }

  public getStyle(): string{
    var style = ""
    if (this.card.resource == 0) {
      style = "#cc9966";
    }
    else if (this.card.resource == 1) {
      style = "#80e5ff";
    }
    else if (this.card.resource == 2) {
      style = "grey";
    }
    else if (this.card.resource == 3) {
      style = "#ffc2b3";
    }
    else if (this.card.resource == 4) {
      style = "#00ff99";
    }
    else if (this.card.resource == 5) {
      style = "#b3b3b3";
    }
    
    return style;
  }
}
