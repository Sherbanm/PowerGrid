import { TestBed, inject } from '@angular/core/testing';

import { CurrentPlayerServiceService } from './current-player-service.service';

describe('CurrentPlayerServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CurrentPlayerServiceService]
    });
  });

  it('should be created', inject([CurrentPlayerServiceService], (service: CurrentPlayerServiceService) => {
    expect(service).toBeTruthy();
  }));
});
