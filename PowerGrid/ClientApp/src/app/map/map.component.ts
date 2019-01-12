import { Component, OnInit, Input } from '@angular/core';
import { Map } from '../domain/map';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  @Input() map: Map;

  constructor() { }

  ngOnInit() {
  }
}