import { Component, OnInit, Input } from '@angular/core';
import { Card} from '../domain/card';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.css']
})
export class CardListComponent implements OnInit {
  @Input() cards: Card[];
  
  constructor() { }

  ngOnInit() {
  }

}
