import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceMarketComponent } from './resource-market.component';

describe('ResourceMarketComponent', () => {
  let component: ResourceMarketComponent;
  let fixture: ComponentFixture<ResourceMarketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceMarketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceMarketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
