import { TestBed, inject } from '@angular/core/testing';

import { GamestateService } from './gamestate.service';

describe('GamestateService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GamestateService]
    });
  });

  it('should be created', inject([GamestateService], (service: GamestateService) => {
    expect(service).toBeTruthy();
  }));
});
