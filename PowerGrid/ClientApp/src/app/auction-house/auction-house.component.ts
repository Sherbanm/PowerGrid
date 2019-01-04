import { Component, OnInit, Input } from '@angular/core';

import { AuctionHouse } from '../domain/auctionHouse';

@Component({
  selector: 'app-auction-house',
  templateUrl: './auction-house.component.html',
  styleUrls: ['./auction-house.component.css']
})
export class AuctionHouseComponent implements OnInit {
  @Input() auctionHouse: AuctionHouse;

  constructor() { }

  ngOnInit() {
  }

}