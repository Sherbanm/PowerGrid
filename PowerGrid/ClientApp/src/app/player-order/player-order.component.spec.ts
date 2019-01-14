import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerOrderComponent } from './player-order.component';

describe('PlayerOrderComponent', () => {
  let component: PlayerOrderComponent;
  let fixture: ComponentFixture<PlayerOrderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerOrderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayerOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
