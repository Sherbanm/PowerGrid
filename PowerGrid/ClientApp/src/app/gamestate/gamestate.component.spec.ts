import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GamestateComponent } from './gamestate.component';

describe('GamestateComponent', () => {
  let component: GamestateComponent;
  let fixture: ComponentFixture<GamestateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GamestateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GamestateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
