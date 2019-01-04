import { Component, OnInit, Input } from '@angular/core';
import { ResourceMarket } from '../domain/resourceMarket';

@Component({
  selector: 'app-resource-market',
  templateUrl: './resource-market.component.html',
  styleUrls: ['./resource-market.component.css']
})
export class ResourceMarketComponent implements OnInit {
  @Input() market: ResourceMarket;
  
  constructor() { }

  ngOnInit() {
  }

}